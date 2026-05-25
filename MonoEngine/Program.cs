using MonoGameTemplate.Managers;
using MonoGameTemplate.GameCode.Scenes;

using var game = new MonoGameTemplate.MainGame();
game.RegisterManager<InputManager>();
game.RegisterManager<AudioManager>();
// RenderManager must come before SceneManager so Camera/MeshRenderer can self-register on scene load
game.RegisterManager<RenderManager>();
var sceneManager = game.RegisterManager<SceneManager>();
sceneManager.LoadScene<DefaultScene>();
game.Run();
