﻿using BulbaCourses.Video.Logic.InterfaceServices;
using BulbaCourses.Video.Web.Models.CourseViews;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulbaCourses.Video.Web.Validators
{
    public class CourseViewValidator : AbstractValidator<CourseView>
    {
        public CourseViewValidator(ICourseService service)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("AddCourse", () => {
                RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(32).WithMessage("Course name is required.");
                RuleFor(c => c.Name).MustAsync((async (title, token) => !(await service.ExistNameAsync(title))))
                    .WithMessage("Name already taken");
                RuleFor(c => c.Level).IsInEnum();
                RuleFor(c => c.Duration).GreaterThan(0).WithMessage("Course duration must be greater than 0.");
                RuleFor(c => c.Price).GreaterThan(0).WithMessage("Course price must be greater than 0.");

            });

            RuleSet("UpdateCourse", () => {
                RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(32).WithMessage("Course name is required.");
                RuleFor(c => c.Name).MustAsync((async (title, token) => !(await service.ExistNameAsync(title))))
                    .WithMessage("Name already taken");
                RuleFor(c => c.Level).IsInEnum();
                RuleFor(c => c.Duration).GreaterThan(0).WithMessage("Course duration must be greater than 0.");
                RuleFor(c => c.Price).GreaterThan(0).WithMessage("Course price must be greater than 0.");
            });
        }
    }
}