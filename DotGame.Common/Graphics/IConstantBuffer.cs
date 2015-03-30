using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public interface IConstantBuffer : IGraphicsObject
    {
        /// <summary>
        /// Die Größe in Bytes.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Updatet den ConstantBuffer mit den neuen Daten.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        void UpdateData<T>(T data) where T : struct;
    }
}
