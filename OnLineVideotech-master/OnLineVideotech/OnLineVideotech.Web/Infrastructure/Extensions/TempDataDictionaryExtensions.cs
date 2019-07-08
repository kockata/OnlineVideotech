using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OnLineVideotech.Web.Infrastructure.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[GlobalConstants.TempDataSuccessMessageKey] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[GlobalConstants.TempDataErrorMessageKey] = message;
        }
    }
}