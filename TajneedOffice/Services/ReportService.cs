using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service for generating PDF reports for candidate evaluations and statistics
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly TajneedOfficeDbContext _context;

        public ReportService(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerateCandidateReportAsync(Guid candidateId)
        {
            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                .FirstOrDefaultAsync(c => c.CandidateId == candidateId);

            if (candidate == null)
                throw new ArgumentException("المرشح غير موجود");

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, stream);

                document.Open();

                // Add candidate information
                AddCandidateInfo(document, candidate);

                // Add test scores if any
                if (candidate.TestResults.Any())
                {
                    AddTestScores(document, candidate.TestResults);
                }

                document.Close();
                return stream.ToArray();
            }
        }

        public async Task<byte[]> GenerateCommitteeSummaryReportAsync(Guid candidateId)
        {
            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .FirstOrDefaultAsync(c => c.CandidateId == candidateId);

            if (candidate == null)
                throw new ArgumentException("المرشح غير موجود");

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, stream);

                document.Open();

                // Add candidate information
                AddCandidateInfo(document, candidate);

                document.Close();
                return stream.ToArray();
            }
        }

        public async Task<byte[]> GenerateFinalResultsReportAsync(int? categoryId = null, bool? isRecommended = null)
        {
            var candidates = await GetCandidatesForReportAsync(categoryId, isRecommended);

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
                PdfWriter.GetInstance(document, stream);

                document.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD);
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);

                Paragraph title = new Paragraph("تقرير النتائج النهائية", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph(" "));

                // Add candidate table
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                AddTableHeader(table, "الاسم", "الرقم الوطني", "الفئة", "الحالة", "الملاحظات");

                foreach (var candidate in candidates)
                {
                    table.AddCell(new PdfPCell(new Phrase(candidate.FullName, FontFactory.GetFont(FontFactory.HELVETICA, 9))));
                    table.AddCell(new PdfPCell(new Phrase(candidate.NationalIdNumber, FontFactory.GetFont(FontFactory.HELVETICA, 9))));
                    table.AddCell(new PdfPCell(new Phrase(candidate.Category?.CategoryName ?? "", FontFactory.GetFont(FontFactory.HELVETICA, 9))));
                    table.AddCell(new PdfPCell(new Phrase(candidate.CurrentStatus, FontFactory.GetFont(FontFactory.HELVETICA, 9))));
                    table.AddCell(new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 9))));
                }

                document.Add(table);

                // Add statistics
                AddStatistics(document, candidates.ToList());

                document.Close();
                return stream.ToArray();
            }
        }

        public async Task<byte[]> GenerateStatisticalReportAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Candidates.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(c => c.DateOfBirth >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(c => c.DateOfBirth <= toDate.Value);

            var candidates = await query
                .Include(c => c.Category)
                .ToListAsync();

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, stream);

                document.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD);

                Paragraph title = new Paragraph("التقرير الإحصائي", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph(" "));

                AddStatistics(document, candidates);

                document.Close();
                return stream.ToArray();
            }
        }

        public async Task<IEnumerable<Candidate>> GetCandidatesForReportAsync(int? categoryId = null, bool? isRecommended = null)
        {
            var query = _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(c => c.CategoryId == categoryId.Value);

            return await query.ToListAsync();
        }

        private void AddCandidateInfo(Document document, Candidate candidate)
        {
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD);
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);

            Paragraph title = new Paragraph("تقرير المرشح", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;

            AddTableRow(table, "الاسم الكامل:", candidate.FullName, headerFont, normalFont);
            AddTableRow(table, "الرقم الوطني:", candidate.NationalIdNumber, headerFont, normalFont);
            AddTableRow(table, "تاريخ الميلاد:", candidate.DateOfBirth.ToString("dd/MM/yyyy"), headerFont, normalFont);
            AddTableRow(table, "المحافظة:", candidate.Governorate, headerFont, normalFont);
            AddTableRow(table, "الفئة:", candidate.Category?.CategoryName ?? "", headerFont, normalFont);
            AddTableRow(table, "الرتبة الحالية:", candidate.CurrentRank?.RankName ?? "", headerFont, normalFont);
            AddTableRow(table, "القاعدة الحالية:", candidate.CurrentAirbase?.AirbaseName ?? "", headerFont, normalFont);

            document.Add(table);
            document.Add(new Paragraph(" "));
        }

        private void AddTestScores(Document document, ICollection<CandidateTestResult> testResults)
        {
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);

            Paragraph header = new Paragraph("درجات الاختبارات", headerFont);
            document.Add(header);

            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            
            // Add table headers
            AddTableHeader(table, "الاختبار", "النتيجة", "الحالة", "تاريخ الاختبار");

            foreach (var result in testResults.OrderBy(tr => tr.TestType.TestName))
            {
                string resultValue = "";
                if (result.TestType.CriteriaType == "درجات" && result.NumericScore.HasValue)
                {
                    resultValue = result.NumericScore.Value.ToString("F1");
                }
                else if (result.TestType.CriteriaType != "درجات" && !string.IsNullOrEmpty(result.TextResult))
                {
                    resultValue = result.TextResult;
                }
                else
                {
                    resultValue = "غير محدد";
                }

                string status = result.IsPassed == true ? "مجتاز" : result.IsPassed == false ? "راسب" : "غير محدد";

                PdfPCell[] cells = {
                    new PdfPCell(new Phrase(result.TestType.TestName, normalFont)),
                    new PdfPCell(new Phrase(resultValue, normalFont)),
                    new PdfPCell(new Phrase(status, normalFont)),
                    new PdfPCell(new Phrase(result.TestDate.ToString("dd/MM/yyyy"), normalFont))
                };

                foreach (var cell in cells)
                {
                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Add(new Paragraph(" "));
        }

        private void AddTableHeader(PdfPTable table, params string[] headers)
        {
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);
            foreach (string header in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new BaseColor(240, 240, 240);
                table.AddCell(cell);
            }
        }

        private void AddStatistics(Document document, List<Candidate> candidates)
        {
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);

            Paragraph header = new Paragraph("الإحصائيات", headerFont);
            document.Add(header);

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;

            var totalCandidates = candidates.Count;
            var activeCandidates = candidates.Count(c => c.IsActive);
            var inactiveCandidates = totalCandidates - activeCandidates;

            AddTableRow(table, "إجمالي المرشحين:", totalCandidates.ToString(), headerFont, normalFont);
            AddTableRow(table, "المرشحين النشطين:", activeCandidates.ToString(), headerFont, normalFont);
            AddTableRow(table, "المرشحين غير النشطين:", inactiveCandidates.ToString(), headerFont, normalFont);

            // Category statistics
            var categoryStats = candidates.GroupBy(c => c.Category?.CategoryName ?? "غير محدد")
                                        .Select(g => new { Category = g.Key, Count = g.Count() });

            foreach (var stat in categoryStats)
            {
                AddTableRow(table, $"عدد {stat.Category}:", stat.Count.ToString(), headerFont, normalFont);
            }

            document.Add(table);
        }

        private void AddTableRow(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            table.AddCell(new PdfPCell(new Phrase(label, labelFont)));
            table.AddCell(new PdfPCell(new Phrase(value, valueFont)));
        }
    }
} 