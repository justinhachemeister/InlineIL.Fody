﻿using InlineIL.Tests.Support;
using Xunit;

namespace InlineIL.Tests.Weaving
{
    public class LabelTests : ClassTestsBase
    {
        public LabelTests()
            : base("LabelTestCases")
        {
        }

        [Fact]
        public void should_handle_labels()
        {
            var result = (int)GetInstance().Branch(false);
            result.ShouldEqual(42);

            result = (int)GetInstance().Branch(true);
            result.ShouldEqual(1);
        }

        [Fact]
        public void should_handle_switch()
        {
            var result = (int)GetInstance().JumpTable(0);
            result.ShouldEqual(1);

            result = (int)GetInstance().JumpTable(1);
            result.ShouldEqual(2);

            result = (int)GetInstance().JumpTable(2);
            result.ShouldEqual(3);

            result = (int)GetInstance().JumpTable(3);
            result.ShouldEqual(42);
        }

        [Fact]
        public void should_report_null_label_reference()
        {
            ShouldHaveError("NullLabelName").ShouldContain("ldnull");
        }

        [Fact]
        public void should_report_null_label_definition()
        {
            ShouldHaveError("NullLabel").ShouldContain("ldnull");
        }

        [Fact]
        public void should_report_undefined_label()
        {
            ShouldHaveError("UndefinedLabel").ShouldContain("Undefined label");
        }

        [Fact]
        public void should_report_redefined_label()
        {
            ShouldHaveError("RedefinedLabel").ShouldContain("already defined");
        }
    }
}
