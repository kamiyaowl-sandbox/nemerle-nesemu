using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nesemu {
	/// <summary>
	/// ゲーム本体の制御を行うクラスです
	/// </summary>
	class GameLoop {
		//DxLibのデフォルトスクリーンサイズは640,480 (DX.SetGraphModeで変更可能
		const int center_x = 320;
		const int center_y = 240;

		double radius = 0;
		const int radius_max = 200;

		/// <summary>
		/// ゲーム起動時に一度だけ呼び出されます
		/// </summary>
		public void Setup() {
		}
		/// <summary>
		/// 敵の移動など計算を行います
		/// </summary>
		public void Calculate() {
			radius = radius_max * Math.Cos(DX.GetNowCount() / 1000.0);//円の半径を変更
		}
		/// <summary>
		/// 描画処理を行います
		/// </summary>
		public void Update() {
			DX.DrawCircle(center_x, center_y, (int)radius, DX.GetColor(0, 0, 255));//青色の円を描画
			DX.DrawString(center_x, center_y, "Hello DxLib.", DX.GetColor(0, 255, 0));
		}
		/// <summary>
		/// ゲーム終了時に呼び出されます。
		/// </summary>
		public void Exit() {
		}
		/// <summary>
		/// ゲームを終了するかどうかの真偽値を返します
		/// </summary>
		/// <returns>ゲームを終了する場合はtrue</returns>
		public bool IsEnd() {
			return DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 1;//ESCキーが押されたら終了する
		}
	}
}
