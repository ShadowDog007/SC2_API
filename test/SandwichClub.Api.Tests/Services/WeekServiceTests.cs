﻿using System;
using System.Threading.Tasks;
using Moq;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Xunit;

namespace SandwichClub.Api.Tests.Services
{
    public class WeekServiceTests
    {
        public class StaticWeekServiceTests : UnitTestBase<WeekService>
        {
            private static readonly DateTime startOfComputerTime = new DateTime(1970, 1, 1);

            [Fact]
            public void GetWeekId_GivenStartOfComputerTime_IdShouldBeZero()
            {
                // When
                var weekId = Service.GetWeekId(startOfComputerTime);

                // Verify
                Assert.Equal(0, weekId);
            }

            [Fact]
            public void GetWeekId_GivenStartOfComputerTimeAndOneWeek_IdShouldBeOne()
            {
                // When
                var weekId = Service.GetWeekId(startOfComputerTime.AddDays(7));

                // Verify
                Assert.Equal(1, weekId);
            }

            [Fact]
            public void GetWeekId_GivenDaysMonToSun_IdShouldNotChange()
            {
                // Given
                // Get a week which should match id 1
                var date = startOfComputerTime.AddDays(7);
                // Translate to the monday of that week
                date = date.AddDays((int)DayOfWeek.Monday-(int)date.DayOfWeek);
                var originalId = Service.GetWeekId(date);

                for (var i = 1; i < 7; ++i)
                {
                    // When
                    var weekId = Service.GetWeekId(date.AddDays(i));

                    // Verify
                    Assert.Equal(originalId, weekId);
                }
            }

            [Fact]
            public async void GetCurrentWeek_ShouldUseWeekIdForToday()
            {
                // Given
                var weekId = Service.GetWeekId(DateTime.Today);
                var repo = Mock<IWeekRepository>();
                repo.Setup(i => i.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Week>(null));

                // When
                await Service.GetCurrentWeekAsync();

                // Verify
                repo.Verify(i => i.GetByIdAsync(weekId), Times.Once);
            }
        }

        public class RepositoryWeekServiceTests : UnitTestBase<WeekService>
        {
            private const int WeekId = 42;

            private readonly IWeekRepository _weekRepo;

            private Week InsertedWeek => new Week {WeekId = WeekId, ShopperUserId = 1, Cost = 10.0};
            private Week UpdatedWeek => new Week {WeekId = WeekId, ShopperUserId = 2, Cost = 20.0};
            private Week DefaultWeek => new Week {WeekId = WeekId, ShopperUserId = null, Cost = 0.0};

            public RepositoryWeekServiceTests()
            {
                _weekRepo = GetRepository<WeekRepository>(); 
            }

            [Fact]
            public async void GetByIdAsync_ShouldProvideDefaultWeek()
            {
                // When
                var week = await Service.GetByIdAsync(WeekId);

                // Verify
                Assert.Equal(WeekId, week.WeekId);
                Assert.Equal(0.0, week.Cost);
                Assert.False(week.ShopperUserId.HasValue);
            }

            [Fact]
            public async void InsertAsync_ShouldInsert()
            {
                // When
                await Service.InsertAsync(InsertedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(InsertedWeek, week);
            }

            [Fact]
            public async void InsertAsync_WhenInsertingAgain_ShouldUpdate()
            {
                // Given
                await _weekRepo.InsertAsync(InsertedWeek);

                // When
                await Service.InsertAsync(UpdatedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(UpdatedWeek, week);
            }

            [Fact]
            public async void InsertAsync_WhenInsertingDefault_ShouldDelete()
            {
                // Given
                await Service.InsertAsync(InsertedWeek);

                // When
                await Service.InsertAsync(DefaultWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                Assert.Null(week);
            }

            [Fact]
            public async void UpdateAsync_ShouldInsert()
            {
                // When
                await Service.UpdateAsync(InsertedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(InsertedWeek, week);
            }

            [Fact]
            public async Task UpdateAsync_WhenUpdatingAgain_ShouldUpdate()
            {
                // Given
                await _weekRepo.InsertAsync(InsertedWeek);

                // When
                await Service.UpdateAsync(UpdatedWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                WeeksEqual(UpdatedWeek, week);
            }

            [Fact]
            public async void UpdateAsync_WhenInsertingDefault_ShouldDelete()
            {
                // Given
                await Service.InsertAsync(InsertedWeek);

                // When
                await Service.UpdateAsync(DefaultWeek);

                // Verify
                var week = await _weekRepo.GetByIdAsync(WeekId);

                Assert.Null(week);
            }

            private void WeeksEqual(Week expected, Week actual)
            {
                Assert.Equal(expected.WeekId, actual.WeekId);
                Assert.Equal(expected.ShopperUserId, actual.ShopperUserId);
                Assert.Equal(expected.Cost, actual.Cost);
            }
        }
    }
}