using System;
using System.Runtime.CompilerServices;
using Reactor;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class AlwaysActive : UnityEngine.MonoBehaviour
    {
        public AlwaysActive(IntPtr ptr) : base(ptr) { }

        public static AlwaysActive Instance;

        private void Start()
        {
            Invoke(nameof(Start_), 0f);
            Instance = this;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void Start_() { }

        private void Update() => Invoke(nameof(Update_), 0f);
        
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void Update_() { }
        
        
        private void FixedUpdate() => Invoke(nameof(FixedUpdate_), 0f);
        
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void FixedUpdate_() { }
        
    }
}