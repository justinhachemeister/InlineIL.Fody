﻿using InlineIL.Tests.Support;
using Xunit;

namespace InlineIL.Tests.Weaving
{
    public class StandAloneMethodSigTests : ClassTestsBase
    {
        public StandAloneMethodSigTests()
            : base("StandAloneMethodSigTestCases")
        {
        }

        [Fact]
        public void should_call_indirect_static()
        {
            var result = (int)GetUnverifiableInstance().CallIndirectStatic();
            result.ShouldEqual(42);
        }

        [Fact]
        public void should_call_indirect_instance()
        {
            var result = (int)GetUnverifiableInstance().CallIndirectInstance();
            result.ShouldEqual(42);
        }

#if NETFWK
        [Fact]
        public void should_call_indirect_vararg()
        {
            var result = (int)GetUnverifiableInstance().CallIndirectVarArg();
            result.ShouldEqual(42);
        }
#endif

        [Fact]
        public void should_report_mismatched_calling_convention()
        {
            ShouldHaveError("InvalidCallingConvention").ShouldContain("Not a vararg calling convention");
        }

        [Fact]
        public void should_report_empty_vararg_params()
        {
            ShouldHaveError("EmptyVarArgParams").ShouldContain("No optional parameter type supplied");
        }

        [Fact]
        public void should_report_vararg_params_supplied_multiple_times()
        {
            ShouldHaveError("VarArgParamsAlreadySupplied").ShouldContain("have already been supplied");
        }

        [Fact]
        public void should_call_indirect_native_stdcall()
        {
            var result = (int)GetUnverifiableInstance().CallIndirectNativeStdcall();
            result.ShouldEqual(42);
        }

        [Fact]
        public void should_call_indirect_native_cdecl()
        {
            var result = (int)GetUnverifiableInstance().CallIndirectNativeCdecl();
            result.ShouldEqual(42);
        }

        [Fact]
        public void should_report_vararg_params_supplied_for_native_call()
        {
            ShouldHaveError("VarArgParamsWithNativeCall").ShouldContain("Not a vararg calling convention");
        }
    }
}
