using Microsoft.SemanticKernel;

namespace AIArbiter;

public interface IKernelFactory
{
    Kernel GetKernel(string modelType);
}