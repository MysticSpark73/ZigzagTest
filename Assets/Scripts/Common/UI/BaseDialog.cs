using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Zagzag.Common.UI
{
    public abstract class BaseDialog : MonoBehaviour
    {
        public bool IsShowing { get; protected set; }

        public abstract Task Show(bool animate = true, Action callback = null);

        public abstract Task Hide(bool animate = true, Action callback = null);

        public virtual async Task Init(bool animate = true, Action callback = null) 
        {
            await Show(animate, callback);
        }
    }
}
