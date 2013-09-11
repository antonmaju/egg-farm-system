using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using Xunit;

namespace EggFarmSystem.Core.Tests.Model
{
    public class HenDepreciationTest
    {
        [Fact]
        public void Should_HaveDetails()
        {
            var info = new HenDepreciation();
            var errors = info.Validate();

            Assert.True(errors.Any(e => e.Message == "HenDepreciation_RequireDetails"));
        }

        [Fact]
        public void Should_ConsistOf_DifferentHouses()
        {
            var houseId = Guid.NewGuid();

            var info = new HenDepreciation
                {
                    Date = DateTime.Today,
                    Details = new List<HenDepreciationDetail>()
                        {
                            new HenDepreciationDetail {HouseId = houseId},
                            new HenDepreciationDetail {HouseId = houseId}
                        }
                };

            var errors = info.Validate();

            Assert.True(errors.Any(e => e.Message == "HenDepreciation_DuplicateHouse"));
        }

        [Fact]
        public void Detail_ShouldHave_HouseId()
        {
            var detail = new HenDepreciationDetail {InitialPrice = 10, SellingPrice = 20, Depreciation = 10};
            var errors = detail.Validate();

            Assert.True(errors.Any(e => e.Message == "HenDepreciationDetail_RequireHouseId"));
        }

        [Fact]
        public void Detail_ShouldHave_InitialPrice()
        {
            var detail = new HenDepreciationDetail { HouseId = Guid.NewGuid(), InitialPrice = -10, SellingPrice = 20, Depreciation = 10 };
            var errors = detail.Validate();

            Assert.True(errors.Any(e => e.Message == "HenDepreciationDetail_InvalidInitialPrice"));
        }

        [Fact]
        public void Detail_ShouldHave_SellingPrice()
        {
            var detail = new HenDepreciationDetail { HouseId = Guid.NewGuid(), InitialPrice = 10, SellingPrice = -20, Depreciation = 10 };
            var errors = detail.Validate();

            Assert.True(errors.Any(e => e.Message == "HenDepreciationDetail_InvalidSellingPrice"));
        }
    }
}
