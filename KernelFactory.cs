using Microsoft.SemanticKernel;

namespace AIArbiter;

public class KernelFactory : IKernelFactory
{
    private readonly Dictionary<string, Kernel> _kernels;

    public KernelFactory(Dictionary<string, Kernel> kernels)
    {
        _kernels = kernels;
    }

    public Kernel GetKernel(string modelType)
    {
        if (_kernels.TryGetValue(modelType.ToLower(), out var kernel))
        {
            return kernel;
        }
        throw new ArgumentException("Invalid model type specified", nameof(modelType));
    }
}