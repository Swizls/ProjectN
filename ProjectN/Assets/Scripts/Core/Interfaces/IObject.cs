using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IObject
{
    public bool IsPassable { get; }
    public bool CanShootThrough { get; }
}
