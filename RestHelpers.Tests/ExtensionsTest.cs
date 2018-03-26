using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestHelpers.Attributes;
using RestHelpers.Extensions;
using RestHelpers.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RestHelpers.Tests
{
    /// <summary>
    ///     Extensions test.
    /// </summary>
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void DataShaping_Select_Success()
        {
            // ARRANGE
            var testModel = new UserTestModel();
            var customSelect = new string[] { "username", "email" };

            // ACT
            var result = testModel.ShapeData(customSelect) as IDictionary<string, object>;

            // ASSERT
            var requiredDto = new List<string>(customSelect.Length);
            var userTestModelProps = typeof(UserTestModel).GetTypeInfo()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in userTestModelProps)
            {
                if (property.GetCustomAttribute(typeof(DtoRequiredAttribute)) != null)
                {
                    requiredDto.Add(property.Name.ToLower());
                }
            }
            var toCompare = requiredDto.Union(customSelect);

            foreach (var item in result.Keys)
            {
                Assert.IsTrue(toCompare.Contains(item.ToLower()));
            }
        }
    }
}
