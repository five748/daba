using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData<T> where T: ITable {
	public Dictionary<int, T> data;
}
