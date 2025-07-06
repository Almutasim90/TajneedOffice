# TajneedOffice - Royal Air Force of Oman Candidate Management System

## Overview

TajneedOffice is a comprehensive candidate management system designed for the Royal Air Force of Oman (RAFO). The system manages the recruitment and evaluation process for various candidate categories including pilots, military university graduates, civilian university graduates, and non-commissioned officers.

## Features

### Core Functionality
- **Candidate Management**: Complete CRUD operations for candidate profiles
- **Category Management**: Support for multiple candidate categories (Pilots, Military/Civilian University Graduates, NCOs, etc.)
- **Flexible Test System**: Configurable test paths with weighted scoring
- **Rank Management**: Military and civilian rank hierarchies
- **Airbase Management**: RAFO airbase locations and assignments
- **User Management**: Role-based access control
- **Reporting**: Comprehensive reporting and analytics

### Candidate Categories Supported
- **CAT-PLT**: Pilot Candidates (المرشحيين الطيارين)
- **CAT-MUG**: Military University Graduates (المرشحيين الجامعيين العسكريين)
- **CAT-CUG**: Civilian University Graduates (المرشحيين الجامعيين المدنيين)
- **CAT-LSO**: Limited Service Officers (ضباط الخدمة المحدودة)
- **CAT-NCO**: Non-Commissioned Officers (ضباط الصف)
- **CAT-TCN**: Technical Military College Graduates (ضباط الصف الكلية التقنية العسكرية)
- **CAT-CNP**: Civilian NCOs for Promotion (ضباط الصف المدنيين للترفيع)

### Test Types
- English Language Test (اختبار اللغة الإنجليزية)
- Arabic Language Test (اختبار اللغة العربية)
- Mathematics Test (اختبار الرياضيات)
- Physical Fitness Test (اختبار اللياقة البدنية)
- Medical Examination (الفحص الطبي)
- Flight Competency Test (اختبار الكفاءة الطيرانية)
- Personal Interview (اختبار المقابلة الشخصية)
- General Knowledge Test (اختبار المعلومات العامة)
- Technical Specialty Test (اختبار التخصص التقني)
- Leadership Skills Assessment (تقييم مهارات القيادة)

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: Entity Framework Core with SQL Server
- **Frontend**: Razor Pages with Bootstrap 5
- **DataTables**: For enhanced data display and filtering
- **Localization**: Arabic and English support
- **Authentication**: ASP.NET Core Identity

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/TajneedOffice.git
cd TajneedOffice
```

### 2. Database Setup
1. Update the connection string in `TajneedOffice/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TajneedOffice;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

2. Run the application - the database will be automatically created and seeded with initial data.

### 3. Build and Run
```bash
cd TajneedOffice
dotnet restore
dotnet build
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Database Seeding

The application includes comprehensive database seeding that provides:

- **Candidate Categories**: All 7 RAFO candidate categories
- **Ranks**: Complete military and civilian rank hierarchy (34 ranks)
- **Airbases**: All 9 RAFO airbases
- **Test Types**: 10 different test types with various criteria
- **Category Test Paths**: Pre-configured test paths for each category with appropriate weights
- **Demo Candidates**: 25 sample candidates for testing

## Project Structure

```
TajneedOffice/
├── Controllers/          # MVC Controllers
├── Data/                # Database context and migrations
├── Models/              # Entity models
├── Services/            # Business logic services
├── ViewModels/          # View models for data binding
├── Views/               # Razor views
├── wwwroot/             # Static files (CSS, JS, images)
├── Attributes/          # Custom validation attributes
├── Helpers/             # Utility classes
└── Migrations/          # Entity Framework migrations
```

## Key Features

### Flexible Test System
- Configurable test paths per candidate category
- Weighted scoring system
- Mandatory and optional tests
- Multiple criteria types (scores, pass/fail, fit/unfit)

### Multi-language Support
- Arabic and English interface
- Right-to-left (RTL) layout support
- Localized content and messages

### Security
- Role-based access control
- Secure authentication
- Input validation and sanitization

### Reporting
- Candidate evaluation reports
- Test result analytics
- Category-wise statistics

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is proprietary software developed for the Royal Air Force of Oman.

## Support

For support and questions, please contact the development team or create an issue in the repository.

## Version History

- **v1.0.0**: Initial release with core candidate management functionality
- **v1.1.0**: Added flexible test system and category test paths
- **v1.2.0**: Enhanced reporting and analytics features

---

**Note**: This system is designed specifically for the Royal Air Force of Oman's recruitment and evaluation processes. All data and processes are aligned with RAFO's operational requirements and security standards. 