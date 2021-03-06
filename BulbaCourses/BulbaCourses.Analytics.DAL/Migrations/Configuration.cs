using System.Collections.Generic;

namespace BulbaCourses.Analytics.DAL.Migrations
{
    using BulbaCourses.Analytics.DAL.Models;
    using System;
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<BulbaCourses.Analytics.DAL.Context.AnalyticsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BulbaCourses.Analytics.DAL.Context.AnalyticsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var reportId = Guid.NewGuid().ToString();
            var report = new ReportDb()
            {
                Id = reportId,
                Name = "�������� �����",
                Description = "�������� ��������� ������",
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Creator = "��������� ������",
                Modifier = "����������� ������",
                Dashboards = new List<DashboardDb>() {
                    new DashboardDb()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ChartId = 1,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Creator = "��������� ��������",
                        Modifier = "����������� ��������",
                        Name = "������� 1",
                        ReportId = reportId
                    }
                }
            };
            var cart = new ChartDb()
            {
                Id = 1,
                Name = "Line"
            };
            context.Reports.Add(report);
            context.Charts.Add(cart);
            context.SaveChanges();
        }
    } 
}
