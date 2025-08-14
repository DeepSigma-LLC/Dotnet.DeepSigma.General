using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics.Tensors;

namespace DeepSigma.General.Utilities
{
    public static class LinearAlgebraUtilities
    {
        public static float GetVectorCosineSimilarty(float[] vector1, float[] vector2)
        {
            return TensorPrimitives.CosineSimilarity(vector1, vector2);
        }
        
    }
}
