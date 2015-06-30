using AnyGameEngine.GameData;
using AnyGameEngine.SaveData;
namespace AnyGameEngine.Modules.Expressions {
	public interface IEvaluate {
		string Evaluate (Game game, Save save);
	}
}
