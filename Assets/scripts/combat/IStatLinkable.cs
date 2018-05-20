using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatLinkable {
    int StatLinkerValue { get; }

    void AddLinker(RPGStatLinker linker);
    void ClearLinkers();
    void UpdateLinkers();
}
