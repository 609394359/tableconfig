/*
* ==============================================================================
*
* FileName: MinCollectionCountAttribute.cs
* Created: 2026/1/2 11:51:37
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Attributes
{
    public class MinCollectionCountAttribute : ValidationAttribute
    {
        private readonly int _minCount;
        public MinCollectionCountAttribute(int minCount)
        {
            _minCount = minCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_minCount > 0 && value == null)
                return new ValidationResult(ErrorMessage ?? $"至少需要 {_minCount} 个元素");

            if (value is ICollection<object> collection)
            {
                if (collection.Count < _minCount)
                {
                    return new ValidationResult(ErrorMessage ?? $"至少需要 {_minCount} 个元素");
                }
            }
            return ValidationResult.Success;
        }
    }
}
