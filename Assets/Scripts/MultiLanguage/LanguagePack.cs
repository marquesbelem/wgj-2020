using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LanguagePack : ScriptableObject {

    public string languageID;
    [Serializable] public class Term {
        public string identifier;
        public string value;
    }
    public List<Term> terms;
    public string this[string termIdentifier] {
        get {
            Term term = terms.Find(t => t.identifier == termIdentifier);
            if (term != null) {
                return term.value;
            }
            return "";
        }
    }

}
