//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------


namespace Microsoft.IdentityModel.Samples.TrustChannel
{
    /// <summary>
    /// The CalculatorService implementation
    /// </summary>
    public class CalculatorService : ICalculator
    {        
        public double Add( double n1, double n2 )
        {
            double result = n1 + n2;
            return result;
        }

        public double Subtract( double n1, double n2 )
        {
            double result = n1 - n2;
            return result;
        }

        public double Multiply( double n1, double n2 )
        {
            double result = n1 * n2;
            return result;
        }

        public double Divide( double n1, double n2 )
        {
            double result = n1 / n2;
            return result;
        }
    }
}

