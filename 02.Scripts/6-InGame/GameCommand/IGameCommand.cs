
using System.Collections;
using EnumTypes;
using Structs;

public interface IUnitCommand
{
    Unit GetUnit();
    CommandContext GetContext();
    
    IEnumerator Execute();
    
    // TODO: 언두 기능 개발
    // void Undo(); 
}