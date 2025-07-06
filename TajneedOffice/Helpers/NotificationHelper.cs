using Microsoft.AspNetCore.Mvc;

namespace TajneedOffice.Helpers
{
    /// <summary>
    /// Helper class for managing global notifications
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// Add a success notification message
        /// </summary>
        public static void AddSuccessNotification(this Controller controller, string message)
        {
            controller.TempData["SuccessMessage"] = message;
        }

        /// <summary>
        /// Add an error notification message
        /// </summary>
        public static void AddErrorNotification(this Controller controller, string message)
        {
            controller.TempData["ErrorMessage"] = message;
        }

        /// <summary>
        /// Add a warning notification message
        /// </summary>
        public static void AddWarningNotification(this Controller controller, string message)
        {
            controller.TempData["WarningMessage"] = message;
        }

        /// <summary>
        /// Add an info notification message
        /// </summary>
        public static void AddInfoNotification(this Controller controller, string message)
        {
            controller.TempData["InfoMessage"] = message;
        }
    }
} 