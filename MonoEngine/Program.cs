using MonoGameTemplate.Managers;
using MonoGameTemplate.GameCode.Scenes;

using var game = new MonoGameTemplate.MainGame();
game.RegisterManager<InputManager>();
game.RegisterManager<AudioManager>();
var sceneManager = game.RegisterManager<SceneManager>();
sceneManager.LoadScene<DefaultScene>();
game.Run();
