using System.Reflection;

namespace TestAllPipelines2.WorkerServices
{
    public static class WorkerAssemblyInfo
    {
        public static Assembly Value { get; } = typeof(WorkerAssemblyInfo).Assembly;
        public static string ServiceName { get; } = "TestAllPipelines2";
    }
}
