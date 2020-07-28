using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class DataStructureUtils  {

		public static bool ContainsSame<T>(List<T> A, List<T> B)
		{
			bool bContain = (A.Count==B.Count);
			if (!bContain)
				return false;

			if (bContain) {
				bool bRecurA = HasRecuringElem<T> (A);
				bool bRecurB = HasRecuringElem<T> (B);
				bContain = !(bRecurA || bRecurB);
			}
			if (!bContain)
				return false;

			foreach (T ema in A) {
				bContain = bContain && B.Contains (ema);
				if (!bContain) {
					return false;
				}
			}

			return true;
		}

		public static bool HasRecuringElem<T>(List<T> A)
		{
			for (int i = 0; i < A.Count; i++) {
				T a = A [i];
				for (int j = i + 1; j < A.Count; j++) {
					T b = A [j];
					if (a.Equals(b)) {
						return true;
					}
				}
			}
			return false;
		}


	}

}