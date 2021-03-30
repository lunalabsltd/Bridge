using Bridge;
using System.Collections;

namespace TestProject1
{
    class TestClassA
    {
        public int Value1 { get; set; }
    }

    public class TestPizda
    {
        public System.Collections.IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(CoroutineThree());
            yield return StartCoroutine(CoroutineThree(), false);
            yield return CoroutineTwo();
        }

        public System.Collections.IEnumerator CoroutineTwo()
        {
            yield return null;
        }

        public System.Collections.IEnumerator CoroutineThree()
        {
            yield return null;
        }

        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return new Coroutine(enumerator);
        }

        public Coroutine StartCoroutine(IEnumerator enumerator, bool xyi)
        {
            return new Coroutine(enumerator, xyi);
        }
    }

    public class Coroutine : YieldInstruction
    {

        private readonly IEnumerator enumerator;
        private bool waitsForFixedUpdate;
        private bool waitsForEndOfFrame;
        private bool firedThisFrameBeforeUpdate;

        internal Coroutine(IEnumerator enumerator, bool xyi)
        {
            this.enumerator = enumerator;
            firedThisFrameBeforeUpdate = true;
            waitsForEndOfFrame = xyi;
        }

        internal Coroutine(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            firedThisFrameBeforeUpdate = true;
        }
    }

        /// <summary>
        /// <para>Base class for all yield instructions.</para>
        /// </summary>
        public class YieldInstruction {

        [ Convention( Notation.CamelCase ) ]
        internal bool IsDone { get; set; }

        internal virtual void Update( float delta ) { }

        /// <summary>
        /// Custom method to reset coroutines
        /// </summary>
        public virtual void Reset() {
            IsDone = false;
        }
    }

    public class WaitForSeconds : YieldInstruction {
        private float waitTime;
        private float time;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UnityEngine.WaitForSeconds"/> class.
        /// </summary>
        /// <param name="delay">Delay.</param>
        public WaitForSeconds( float delay ) {
            waitTime = delay;
            Reset();
        }

        internal override void Update( float delta ) {

        }

        /// <summary>
        /// Resets time of wait object for reuse in user code
        /// </summary>
        public override void Reset() {
            time = waitTime;
            IsDone = false;
        }
    }
}
