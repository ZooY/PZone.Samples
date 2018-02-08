using System;
using Newtonsoft.Json.Serialization;
using Ninject;


namespace PZone.Samples
{
    /// <summary>
    /// Ninject contract resolver
    /// </summary>
    public class NinjectContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// The <see cref="IKernel"/>.
        /// </summary>
        private readonly IKernel _kernel;


        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContractResolver"/> class.
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/>.</param>
        public NinjectContractResolver(IKernel kernel)
        {
            _kernel = kernel;
        }


        /// <summary>
        /// Resolve contract by Ninject kernel.
        /// </summary>
        /// <param name="objectType">The contract type.</param>
        /// <returns>The <see cref="JsonObjectContract"/>.</returns>
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            contract.DefaultCreator = () => _kernel.Get(objectType);
            return contract;
        }
    }
}