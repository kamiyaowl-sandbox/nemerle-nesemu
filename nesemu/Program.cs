using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nesemu {
	/// <summary>
	/// DxLibでゲーム開発するための最小限のコード
	/// fps制御、初期化等をこのクラスで行う。
	/// 命令詳細は
	/// http://homepage2.nifty.com/natupaji/DxLib/dxfunc.html
	/// を参照
	/// </summary>
	class Program {
		static void Main(string[] args) {
			//==============================
			// 起動するゲームループ
			GameLoop game = new GameLoop();
			//==============================

			DX.ChangeWindowMode(DX.TRUE);//ウインドウモード（DX.FALSEでフルスクリーン）
			if (DX.DxLib_Init() == -1) {
				throw new PlatformNotSupportedException("DxLibの初期化に失敗しました。");
			}
			DX.SetDrawScreen(DX.DX_SCREEN_BACK);//描画先グラフィックの指定（ダブルバッファリングを有効化しているためDX_SCREEN_BACKを指定
			DX.SetAlwaysRunFlag(DX.TRUE);//ウインドウが非アクティブでも実行を続ける
			DX.SetWindowText("nes emu");//ウインドウタイトル

			// 3Dを使用するときの設定
			//DX.SetCameraNearFar(100.0f, 2000.0f);
			//DX.SetUseZBuffer3D(DX.TRUE);
			//DX.SetWriteZBuffer3D(DX.TRUE);
			//DX.ChangeLightTypeDir(DX.VGet(0.0f, 0.0f, 1.0f));

			game.Setup();//ゲーム初期化処理

			//=============================
			// fps計測用
			const int fps = 60;
			const int duration = 1000 / fps;//ms

			int idealNextTick = 0;

			int fpsCalcTick = 0;
			int fpsCount = 0;
			float currentFps = 0;

			//============================
			// メインループ
			while (DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0 && !game.IsEnd()) {//game.IsEndがtrueを返した場合も終了する
				#region Calclate
				//============================
				// fps計測
				if (++fpsCount > fps) {
					currentFps = fps * 1000 / (float)(DX.GetNowCount() - fpsCalcTick);
					fpsCalcTick = DX.GetNowCount();
					fpsCount = 0;
				}

				game.Calculate();//ゲーム計算処理
				#endregion

				#region Drawing

				//============================
				// DEBUGビルド時のみfpsを表示しておく
#if DEBUG
				DX.DrawString(10, 10, string.Format("FPS = {0}", currentFps), DX.GetColor(0xff, 0xff, 0xff));
#endif
				game.Update();//ゲーム更新処理

				#endregion

				DX.ScreenFlip();

				#region FPS fix wait
				//============================
				// fps調整用のディレイ
				if (DX.GetNowCount() < idealNextTick) {
					DX.WaitTimer(idealNextTick - DX.GetNowCount());
				}
				idealNextTick = DX.GetNowCount() + duration;
				#endregion
			}

			//============================
			// ゲーム終了
			game.Exit();//ゲーム終了処理
			DX.DxLib_End();
		}
	}
}
