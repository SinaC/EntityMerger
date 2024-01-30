﻿using DeepDiff.Comparers;
using DeepDiff.UnitTest.Entities.Simple;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace DeepDiff.UnitTest.Comparer
{
    public class NaiveEqualityComparerByPropertyCompareTests
    {
        [Fact]
        public void TypeAndPropertyInfoSpecificComparer_6Decimals()
        {
            var comparerFactory = new ComparerFactory<EntityLevel1>();

            var typeSpecificComparers = new Dictionary<Type, IEqualityComparer>
            {
                { typeof(decimal?), NonGenericEqualityComparer.Create(new NullableDecimalComparer(6)) },
            };

            var propertyInfoSpecificComparers = new Dictionary<PropertyInfo, IEqualityComparer>
            {
                { comparerFactory.GetPropertyInfo(x => x.Price), NonGenericEqualityComparer.Create(new NullableDecimalComparer(3)) }
            };

            var comparer = comparerFactory.CreateNaiveComparer(x => x.Price, typeSpecificComparers, propertyInfoSpecificComparers);

            var existingEntity = new EntityLevel1
            {
                Price = 7.1234500000m,
            };
            var calculatedEntity = new EntityLevel1
            {
                Price = 7.1234599999m
            };

            var result = comparer.Compare(existingEntity, calculatedEntity); // decimal(3) will have higher priority than decimal(6) because defined at property level

            Assert.NotNull(result);
            Assert.True(result.IsEqual);
            Assert.Empty(result.Details);
        }

        [Fact]
        public void TypeAndPropertyInfoSpecificComparer_3Decimals()
        {
            var comparerFactory = new ComparerFactory<EntityLevel1>();

            var typeSpecificComparers = new Dictionary<Type, IEqualityComparer>
            {
                { typeof(decimal?), NonGenericEqualityComparer.Create(new NullableDecimalComparer(3)) },
            };

            var propertyInfoSpecificComparers = new Dictionary<PropertyInfo, IEqualityComparer>
            {
                { comparerFactory.GetPropertyInfo(x => x.Price), NonGenericEqualityComparer.Create(new NullableDecimalComparer(6)) }
            };

            var comparer = comparerFactory.CreateNaiveComparer(x => x.Price, typeSpecificComparers, propertyInfoSpecificComparers);

            var existingEntity = new EntityLevel1
            {
                Price = 7.1234500000m,
            };
            var calculatedEntity = new EntityLevel1
            {
                Price = 7.1234599999m
            };

            var result = comparer.Compare(existingEntity, calculatedEntity); // decimal(6) will have higher priority than decimal(3) because defined at property level

            Assert.NotNull(result);
            Assert.False(result.IsEqual);
            Assert.Single(result.Details);
            Assert.Equal(nameof(EntityLevel1.Price), result.Details.Single().PropertyInfo.Name);
            Assert.Equal((object)7.1234500000m, result.Details.Single().OldValue);
            Assert.Equal((object)7.1234599999m, result.Details.Single().NewValue);
        }
    }
}
