using System;


namespace GalloUtils {
    public static partial class EnumUtils {

        public static void ForEachEnumValue<E>(Action<E> action) where E : struct, IConvertible {
            foreach (E e in Enum.GetValues(typeof(E))) {
                action.Invoke(e);
            }
        }

    }

}
