using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgTool
{
    public class datModel
    {
        public int Id { get; set; }
        /////// ayush, branch
        ////public string 印刷順 { get; set; }
        ////public string 受験者ID { get; set; }
        ////public string 学籍番号 { get; set; }
        ////public string カナ氏名 { get; set; }
        ////public string 大学名 { get; set; }
        ////public string 受験日 { get; set; }
        ////public string 前回受験日 { get; set; }
        ////public string 受験形態 { get; set; }
        ////public string リテラシー総合レベル { get; set; }
        ////public string コンピテンシー総合レベル { get; set; }
        ////public string 前回リテラシー総合レベル { get; set; }
        ////public string 前回コンピテンシー総合レベル { get; set; }
        ////public string 可変コメント { get; set; }
        ////public string リテラシー総合判定確率Lv1 { get; set; }
        ////public string リテラシー総合判定確率Lv2 { get; set; }
        ////public string リテラシー総合判定確率Lv3 { get; set; }
        ////public string リテラシー総合判定確率Lv4 { get; set; }
        ////public string リテラシー総合判定確率Lv5 { get; set; }
        ////public string リテラシー総合判定確率Lv6 { get; set; }
        ////public string リテラシー総合判定確率Lv7 { get; set; }
        ////public string 可変コメント1 { get; set; }
        ////public string 情報収集力レベル { get; set; }
        ////public string 情報分析力レベル { get; set; }
        ////public string 課題発見力レベル { get; set; }
        ////public string 構想力レベル { get; set; }
        ////public string 前回情報収集力レベル { get; set; }
        ////public string 前回情報分析力レベル { get; set; }
        ////public string 前回課題発見力レベル { get; set; }
        ////public string 前回構想力レベル { get; set; }
        ////public string 可変コメント2 { get; set; }
        ////public string 言語処理能力レベル { get; set; }
        ////public string 非言語処理能力レベル { get; set; }
        ////public string 可変コメント3 { get; set; }
        ////public string コンピテンシー総合判定確率Lv1 { get; set; }
        ////public string コンピテンシー総合判定確率Lv2 { get; set; }
        ////public string コンピテンシー総合判定確率Lv3 { get; set; }
        ////public string コンピテンシー総合判定確率Lv4 { get; set; }
        ////public string コンピテンシー総合判定確率Lv5 { get; set; }
        ////public string コンピテンシー総合判定確率Lv6 { get; set; }
        ////public string コンピテンシー総合判定確率Lv7 { get; set; }
        ////public string 可変コメント4 { get; set; }
        ////public string 対人基礎力レベル { get; set; }
        ////public string 対自己基礎力レベル { get; set; }
        ////public string 対課題基礎力レベル { get; set; }
        ////public string 前回対人基礎力レベル { get; set; }
        ////public string 前回対自己基礎力レベル { get; set; }
        ////public string 前回対課題基礎力レベル { get; set; }
        ////public string 可変コメント5 { get; set; }
        ////public string 課題発見力レベル1 { get; set; }
        ////public string 計画立案力レベル { get; set; }
        ////public string 実践力レベル { get; set; }
        ////public string 親和力レベル { get; set; }
        ////public string 協働力レベル { get; set; }
        ////public string 統率力レベル { get; set; }
        ////public string 感情制御力レベル { get; set; }
        ////public string 自信創出力レベル { get; set; }
        ////public string 行動持続力レベル { get; set; }
        ////public string 課題発見力強み { get; set; }
        ////public string 計画立案力強み { get; set; }
        ////public string 実践力強み { get; set; }
        ////public string 親和力強み { get; set; }
        ////public string 協働力強み { get; set; }
        ////public string 統率力強み { get; set; }
        ////public string 感情制御力強み { get; set; }
        ////public string 自信創出力強み { get; set; }
        ////public string 行動持続力強み { get; set; }
        ////public string 前回課題発見力レベル2 { get; set; }
        ////public string 前回計画立案力レベル { get; set; }
        ////public string 前回実践力レベル { get; set; }
        ////public string 前回親和力レベル { get; set; }
        ////public string 前回協働力レベル { get; set; }
        ////public string 前回統率力レベル { get; set; }
        ////public string 前回感情制御力レベル { get; set; }
        ////public string 前回自信創出力レベル { get; set; }
        ////public string 前回行動持続力レベル { get; set; }
        ////public string 可変コメント6 { get; set; }
        ////public string num1位小分類名 { get; set; }
        ////public string num1位説明 { get; set; }
        ////public string num1位レベル { get; set; }
        ////public string num2位小分類名 { get; set; }
        ////public string num2位説明 { get; set; }
        ////public string num2位レベル { get; set; }
        ////public string num3位小分類名 { get; set; }
        ////public string num3位説明 { get; set; }
        ////public string num3位レベル { get; set; }
        ////public string num4位小分類名 { get; set; }
        ////public string num4位説明 { get; set; }
        ////public string num4位レベル { get; set; }
        ////public string num5位小分類名 { get; set; }
        ////public string num5位説明 { get; set; }
        ////public string num5位レベル { get; set; }
        ////public string num6位小分類名 { get; set; }
        ////public string num6位説明 { get; set; }
        ////public string num6位レベル { get; set; }
        ////public string num7位小分類名 { get; set; }
        ////public string num7位説明 { get; set; }
        ////public string num7位レベル { get; set; }
        ////public string num8位小分類名 { get; set; }
        ////public string num8位説明 { get; set; }
        ////public string num8位レベル { get; set; }
        ////public string num9位小分類名 { get; set; }
        ////public string num9位説明 { get; set; }
        ////public string num9位レベル { get; set; }
        ////public string num10位小分類名 { get; set; }
        ////public string num10位説明 { get; set; }
        ////public string num10位レベル { get; set; }
        ////public string num11位小分類名 { get; set; }
        ////public string num11位説明 { get; set; }
        ////public string num11位レベル { get; set; }
        ////public string num12位小分類名 { get; set; }
        ////public string num12位説明 { get; set; }
        ////public string num12位レベル { get; set; }
        ////public string num13位小分類名 { get; set; }
        ////public string num13位説明 { get; set; }
        ////public string num13位レベル { get; set; }
        ////public string num14位小分類名 { get; set; }
        ////public string num14位説明 { get; set; }
        ////public string num14位レベル { get; set; }
        ////public string num15位小分類名 { get; set; }
        ////public string num15位説明 { get; set; }
        ////public string num15位レベル { get; set; }
        ////public string num16位小分類名 { get; set; }
        ////public string num16位説明 { get; set; }
        ////public string num16位レベル { get; set; }
        ////public string num17位小分類名 { get; set; }
        ////public string num17位説明 { get; set; }
        ////public string num17位レベル { get; set; }
        ////public string num18位小分類名 { get; set; }
        ////public string num18位説明 { get; set; }
        ////public string num18位レベル { get; set; }
        ////public string num19位小分類名 { get; set; }
        ////public string num19位説明 { get; set; }
        ////public string num19位レベル { get; set; }
        ////public string num20位小分類名 { get; set; }
        ////public string num20位説明 { get; set; }
        ////public string num20位レベル { get; set; }
        ////public string num21位小分類名 { get; set; }
        ////public string num21位説明 { get; set; }
        ////public string num21位レベル { get; set; }
        ////public string num22位小分類名 { get; set; }
        ////public string num22位説明 { get; set; }
        ////public string num22位レベル { get; set; }
        ////public string num23位小分類名 { get; set; }
        ////public string num23位説明 { get; set; }
        ////public string num23位レベル { get; set; }
        ////public string num24位小分類名 { get; set; }
        ////public string num24位説明 { get; set; }
        ////public string num24位レベル { get; set; }
        ////public string num25位小分類名 { get; set; }
        ////public string num25位説明 { get; set; }
        ////public string num25位レベル { get; set; }
        ////public string num26位小分類名 { get; set; }
        ////public string num26位説明 { get; set; }
        ////public string num26位レベル { get; set; }
        ////public string num27位小分類名 { get; set; }
        ////public string num27位説明 { get; set; }
        ////public string num27位レベル { get; set; }
        ////public string num28位小分類名 { get; set; }
        ////public string num28位説明 { get; set; }
        ////public string num28位レベル { get; set; }
        ////public string num29位小分類名 { get; set; }
        ////public string num29位説明 { get; set; }
        ////public string num29位レベル { get; set; }
        ////public string num30位小分類名 { get; set; }
        ////public string num30位説明 { get; set; }
        ////public string num30位レベル { get; set; }
        ////public string num31位小分類名 { get; set; }
        ////public string num31位説明 { get; set; }
        ////public string num31位レベル { get; set; }
        ////public string num32位小分類名 { get; set; }
        ////public string num32位説明 { get; set; }
        ////public string num32位レベル { get; set; }
        ////public string num33位小分類名 { get; set; }
        ////public string num33位説明 { get; set; }
        ////public string num33位レベル { get; set; }
        ////public string 前回課題発見力強み { get; set; }
        ////public string 前回計画立案力強み { get; set; }
        ////public string 前回実践力強み { get; set; }
        ////public string 前回親和力強み { get; set; }
        ////public string 前回協働力強み { get; set; }
        ////public string 前回統率力強み { get; set; }
        ////public string 前回感情制御力強み { get; set; }
        ////public string 前回自信創出力強み { get; set; }
        ////public string 前回行動持続力強み { get; set; }

        //	印刷順			CSV_COL名：	印刷順	
        public string 印刷順 { get; set; }
        //	受験者ID			CSV_COL名：	受験者ID	
        public string 受験者ID { get; set; }
        //	社員職員番号			CSV_COL名：	社員／職員番号	
        public string 社員職員番号 { get; set; }
        //	カナ氏名			CSV_COL名：	カナ氏名	
        public string カナ氏名 { get; set; }
        //	顧客名			CSV_COL名：	顧客名	
        public string 顧客名 { get; set; }
        //	結果管理情報			CSV_COL名：	結果管理情報	
        public string 結果管理情報 { get; set; }
        //	言語商品区分実施テスト			CSV_COL名：	言語・商品区分・実施テスト	
        public string 言語商品区分実施テスト { get; set; }
        //	キャリアステージ			CSV_COL名：	キャリアステージ	
        public string Lキャリアステージコード { get; set; }
        // 
        public string Lキャリアステージ { get; set; }
        //
        public string Cキャリアステージコード { get; set; }
        //
        public string Cキャリアステージ { get; set; }
        //	採点日			CSV_COL名：	採点日	
        public string 採点日 { get; set; }
        //	リテラシー総合レベル			CSV_COL名：	リテラシー総合：レベル	
        public string リテラシー総合レベル { get; set; }
        //	リテラシー総合強化ポイント			CSV_COL名：	リテラシー総合：強化ポイント	
        public string リテラシー総合強化ポイント { get; set; }
        //	リテラシー総合CS平均			CSV_COL名：	リテラシー総合：CS平均	
        public string リテラシー総合CS平均 { get; set; }
        //	リテラシー総合CS帯1			CSV_COL名：	リテラシー総合：CS帯1	
        public string リテラシー総合CS帯1 { get; set; }
        //	リテラシー総合CS帯2			CSV_COL名：	リテラシー総合：CS帯2	
        public string リテラシー総合CS帯2 { get; set; }
        //	リテラシー総合CS帯3			CSV_COL名：	リテラシー総合：CS帯3	
        public string リテラシー総合CS帯3 { get; set; }
        //	リテラシー総合CS帯4			CSV_COL名：	リテラシー総合：CS帯4	
        public string リテラシー総合CS帯4 { get; set; }
        //	リテラシー総合CS帯5			CSV_COL名：	リテラシー総合：CS帯5	
        public string リテラシー総合CS帯5 { get; set; }
        //	リテラシー情報収集力レベル			CSV_COL名：	リテラシー情報収集力：レベル	
        public string リテラシー情報収集力レベル { get; set; }
        //	リテラシー情報分析力レベル			CSV_COL名：	リテラシー情報分析力：レベル	
        public string リテラシー情報分析力レベル { get; set; }
        //	リテラシー課題発見力レベル			CSV_COL名：	リテラシー課題発見力：レベル	
        public string リテラシー課題発見力レベル { get; set; }
        //	リテラシー構想力レベル			CSV_COL名：	リテラシー構想力：レベル	
        public string リテラシー構想力レベル { get; set; }
        //	リテラシー情報収集力強化ポイント			CSV_COL名：	リテラシー情報収集力：強化ポイント	
        public string リテラシー情報収集力強化ポイント { get; set; }
        //	リテラシー情報分析力強化ポイント			CSV_COL名：	リテラシー情報分析力：強化ポイント	
        public string リテラシー情報分析力強化ポイント { get; set; }
        //	リテラシー課題発見力強化ポイント			CSV_COL名：	リテラシー課題発見力：強化ポイント	
        public string リテラシー課題発見力強化ポイント { get; set; }
        //	リテラシー構想力強化ポイント			CSV_COL名：	リテラシー構想力：強化ポイント	
        public string リテラシー構想力強化ポイント { get; set; }
        //	リテラシー情報収集力CS平均			CSV_COL名：	リテラシー情報収集力：CS平均	
        public string リテラシー情報収集力CS平均 { get; set; }
        //	リテラシー情報分析力CS平均			CSV_COL名：	リテラシー情報分析力：CS平均	
        public string リテラシー情報分析力CS平均 { get; set; }
        //	リテラシー課題発見力CS平均			CSV_COL名：	リテラシー課題発見力：CS平均	
        public string リテラシー課題発見力CS平均 { get; set; }
        //	リテラシー構想力CS平均			CSV_COL名：	リテラシー構想力：CS平均	
        public string リテラシー構想力CS平均 { get; set; }
        //	リテラシー情報収集力CS帯1			CSV_COL名：	リテラシー情報収集力：CS帯1	
        public string リテラシー情報収集力CS帯1 { get; set; }
        //	リテラシー情報収集力CS帯2			CSV_COL名：	リテラシー情報収集力：CS帯2	
        public string リテラシー情報収集力CS帯2 { get; set; }
        //	リテラシー情報収集力CS帯3			CSV_COL名：	リテラシー情報収集力：CS帯3	
        public string リテラシー情報収集力CS帯3 { get; set; }
        //	リテラシー情報収集力CS帯4			CSV_COL名：	リテラシー情報収集力：CS帯4	
        public string リテラシー情報収集力CS帯4 { get; set; }
        //	リテラシー情報収集力CS帯5			CSV_COL名：	リテラシー情報収集力：CS帯5	
        public string リテラシー情報収集力CS帯5 { get; set; }
        //	リテラシー情報分析力CS帯1			CSV_COL名：	リテラシー情報分析力：CS帯1	
        public string リテラシー情報分析力CS帯1 { get; set; }
        //	リテラシー情報分析力CS帯2			CSV_COL名：	リテラシー情報分析力：CS帯2	
        public string リテラシー情報分析力CS帯2 { get; set; }
        //	リテラシー情報分析力CS帯3			CSV_COL名：	リテラシー情報分析力：CS帯3	
        public string リテラシー情報分析力CS帯3 { get; set; }
        //	リテラシー情報分析力CS帯4			CSV_COL名：	リテラシー情報分析力：CS帯4	
        public string リテラシー情報分析力CS帯4 { get; set; }
        //	リテラシー情報分析力CS帯5			CSV_COL名：	リテラシー情報分析力：CS帯5	
        public string リテラシー情報分析力CS帯5 { get; set; }
        //	リテラシー課題発見力CS帯1			CSV_COL名：	リテラシー課題発見力：CS帯1	
        public string リテラシー課題発見力CS帯1 { get; set; }
        //	リテラシー課題発見力CS帯2			CSV_COL名：	リテラシー課題発見力：CS帯2	
        public string リテラシー課題発見力CS帯2 { get; set; }
        //	リテラシー課題発見力CS帯3			CSV_COL名：	リテラシー課題発見力：CS帯3	
        public string リテラシー課題発見力CS帯3 { get; set; }
        //	リテラシー課題発見力CS帯4			CSV_COL名：	リテラシー課題発見力：CS帯4	
        public string リテラシー課題発見力CS帯4 { get; set; }
        //	リテラシー課題発見力CS帯5			CSV_COL名：	リテラシー課題発見力：CS帯5	
        public string リテラシー課題発見力CS帯5 { get; set; }
        //	リテラシー構想力CS帯1			CSV_COL名：	リテラシー構想力：CS帯1	
        public string リテラシー構想力CS帯1 { get; set; }
        //	リテラシー構想力CS帯2			CSV_COL名：	リテラシー構想力：CS帯2	
        public string リテラシー構想力CS帯2 { get; set; }
        //	リテラシー構想力CS帯3			CSV_COL名：	リテラシー構想力：CS帯3	
        public string リテラシー構想力CS帯3 { get; set; }
        //	リテラシー構想力CS帯4			CSV_COL名：	リテラシー構想力：CS帯4	
        public string リテラシー構想力CS帯4 { get; set; }
        //	リテラシー構想力CS帯5			CSV_COL名：	リテラシー構想力：CS帯5	
        public string リテラシー構想力CS帯5 { get; set; }
        //	コンピテンシー総合レベル			CSV_COL名：	コンピテンシー総合：レベル	
        public string コンピテンシー総合レベル { get; set; }
        //	コンピテンシー総合強化ポイント			CSV_COL名：	コンピテンシー総合：強化ポイント	
        public string コンピテンシー総合強化ポイント { get; set; }
        //	コンピテンシー総合CS平均			CSV_COL名：	コンピテンシー総合：CS平均	
        public string コンピテンシー総合CS平均 { get; set; }
        //	コンピテンシー総合CS帯1			CSV_COL名：	コンピテンシー総合：CS帯1	
        public string コンピテンシー総合CS帯1 { get; set; }
        //	コンピテンシー総合CS帯2			CSV_COL名：	コンピテンシー総合：CS帯2	
        public string コンピテンシー総合CS帯2 { get; set; }
        //	コンピテンシー総合CS帯3			CSV_COL名：	コンピテンシー総合：CS帯3	
        public string コンピテンシー総合CS帯3 { get; set; }
        //	コンピテンシー総合CS帯4			CSV_COL名：	コンピテンシー総合：CS帯4	
        public string コンピテンシー総合CS帯4 { get; set; }
        //	コンピテンシー総合CS帯5			CSV_COL名：	コンピテンシー総合：CS帯5	
        public string コンピテンシー総合CS帯5 { get; set; }
        //	対人基礎力レベル			CSV_COL名：	対人基礎力：レベル	
        public string 対人基礎力レベル { get; set; }
        //	対自己基礎力レベル			CSV_COL名：	対自己基礎力：レベル	
        public string 対自己基礎力レベル { get; set; }
        //	対課題基礎力レベル			CSV_COL名：	対課題基礎力：レベル	
        public string 対課題基礎力レベル { get; set; }
        //	対人基礎力強化ポイント			CSV_COL名：	対人基礎力：強化ポイント	
        public string 対人基礎力強化ポイント { get; set; }
        //	対自己基礎力強化ポイント			CSV_COL名：	対自己基礎力：強化ポイント	
        public string 対自己基礎力強化ポイント { get; set; }
        //	対課題基礎力強化ポイント			CSV_COL名：	対課題基礎力：強化ポイント	
        public string 対課題基礎力強化ポイント { get; set; }
        //	対人基礎力CS平均			CSV_COL名：	対人基礎力：CS平均	
        public string 対人基礎力CS平均 { get; set; }
        //	対自己基礎力CS平均			CSV_COL名：	対自己基礎力：CS平均	
        public string 対自己基礎力CS平均 { get; set; }
        //	対課題基礎力CS平均			CSV_COL名：	対課題基礎力：CS平均	
        public string 対課題基礎力CS平均 { get; set; }
        //	対人基礎力CS帯1			CSV_COL名：	対人基礎力：CS帯1	
        public string 対人基礎力CS帯1 { get; set; }
        //	対人基礎力CS帯2			CSV_COL名：	対人基礎力：CS帯2	
        public string 対人基礎力CS帯2 { get; set; }
        //	対人基礎力CS帯3			CSV_COL名：	対人基礎力：CS帯3	
        public string 対人基礎力CS帯3 { get; set; }
        //	対人基礎力CS帯4			CSV_COL名：	対人基礎力：CS帯4	
        public string 対人基礎力CS帯4 { get; set; }
        //	対人基礎力CS帯5			CSV_COL名：	対人基礎力：CS帯5	
        public string 対人基礎力CS帯5 { get; set; }
        //	対自己基礎力CS帯1			CSV_COL名：	対自己基礎力：CS帯1	
        public string 対自己基礎力CS帯1 { get; set; }
        //	対自己基礎力CS帯2			CSV_COL名：	対自己基礎力：CS帯2	
        public string 対自己基礎力CS帯2 { get; set; }
        //	対自己基礎力CS帯3			CSV_COL名：	対自己基礎力：CS帯3	
        public string 対自己基礎力CS帯3 { get; set; }
        //	対自己基礎力CS帯4			CSV_COL名：	対自己基礎力：CS帯4	
        public string 対自己基礎力CS帯4 { get; set; }
        //	対自己基礎力CS帯5			CSV_COL名：	対自己基礎力：CS帯5	
        public string 対自己基礎力CS帯5 { get; set; }
        //	対課題基礎力CS帯1			CSV_COL名：	対課題基礎力：CS帯1	
        public string 対課題基礎力CS帯1 { get; set; }
        //	対課題基礎力CS帯2			CSV_COL名：	対課題基礎力：CS帯2	
        public string 対課題基礎力CS帯2 { get; set; }
        //	対課題基礎力CS帯3			CSV_COL名：	対課題基礎力：CS帯3	
        public string 対課題基礎力CS帯3 { get; set; }
        //	対課題基礎力CS帯4			CSV_COL名：	対課題基礎力：CS帯4	
        public string 対課題基礎力CS帯4 { get; set; }
        //	対課題基礎力CS帯5			CSV_COL名：	対課題基礎力：CS帯5	
        public string 対課題基礎力CS帯5 { get; set; }
        //	親和力レベル			CSV_COL名：	親和力：レベル	
        public string 親和力レベル { get; set; }
        //	協働力レベル			CSV_COL名：	協働力：レベル	
        public string 協働力レベル { get; set; }
        //	統率力レベル			CSV_COL名：	統率力：レベル	
        public string 統率力レベル { get; set; }
        //	感情制御力レベル			CSV_COL名：	感情制御力：レベル	
        public string 感情制御力レベル { get; set; }
        //	自信創出力レベル			CSV_COL名：	自信創出力：レベル	
        public string 自信創出力レベル { get; set; }
        //	行動持続力レベル			CSV_COL名：	行動持続力：レベル	
        public string 行動持続力レベル { get; set; }
        //	課題発見力レベル			CSV_COL名：	課題発見力：レベル	
        public string 課題発見力レベル { get; set; }
        //	計画立案力レベル			CSV_COL名：	計画立案力：レベル	
        public string 計画立案力レベル { get; set; }
        //	実践力レベル			CSV_COL名：	実践力：レベル	
        public string 実践力レベル { get; set; }
        //	親和力強化ポイント			CSV_COL名：	親和力：強化ポイント	
        public string 親和力強化ポイント { get; set; }
        //	協働力強化ポイント			CSV_COL名：	協働力：強化ポイント	
        public string 協働力強化ポイント { get; set; }
        //	統率力強化ポイント			CSV_COL名：	統率力：強化ポイント	
        public string 統率力強化ポイント { get; set; }
        //	感情制御力強化ポイント			CSV_COL名：	感情制御力：強化ポイント	
        public string 感情制御力強化ポイント { get; set; }
        //	自信創出力強化ポイント			CSV_COL名：	自信創出力：強化ポイント	
        public string 自信創出力強化ポイント { get; set; }
        //	行動持続力強化ポイント			CSV_COL名：	行動持続力：強化ポイント	
        public string 行動持続力強化ポイント { get; set; }
        //	課題発見力強化ポイント			CSV_COL名：	課題発見力：強化ポイント	
        public string 課題発見力強化ポイント { get; set; }
        //	計画立案力強化ポイント			CSV_COL名：	計画立案力：強化ポイント	
        public string 計画立案力強化ポイント { get; set; }
        //	実践力強化ポイント			CSV_COL名：	実践力：強化ポイント	
        public string 実践力強化ポイント { get; set; }
        //	親和力CS平均			CSV_COL名：	親和力：CS平均	
        public string 親和力CS平均 { get; set; }
        //	協働力CS平均			CSV_COL名：	協働力：CS平均	
        public string 協働力CS平均 { get; set; }
        //	統率力CS平均			CSV_COL名：	統率力：CS平均	
        public string 統率力CS平均 { get; set; }
        //	感情制御力CS平均			CSV_COL名：	感情制御力：CS平均	
        public string 感情制御力CS平均 { get; set; }
        //	自信創出力CS平均			CSV_COL名：	自信創出力：CS平均	
        public string 自信創出力CS平均 { get; set; }
        //	行動持続力CS平均			CSV_COL名：	行動持続力：CS平均	
        public string 行動持続力CS平均 { get; set; }
        //	課題発見力CS平均			CSV_COL名：	課題発見力：CS平均	
        public string 課題発見力CS平均 { get; set; }
        //	計画立案力CS平均			CSV_COL名：	計画立案力：CS平均	
        public string 計画立案力CS平均 { get; set; }
        //	実践力CS平均			CSV_COL名：	実践力：CS平均	
        public string 実践力CS平均 { get; set; }
        //	親和力CS帯1			CSV_COL名：	親和力：CS帯1	
        public string 親和力CS帯1 { get; set; }
        //	親和力CS帯2			CSV_COL名：	親和力：CS帯2	
        public string 親和力CS帯2 { get; set; }
        //	親和力CS帯3			CSV_COL名：	親和力：CS帯3	
        public string 親和力CS帯3 { get; set; }
        //	親和力CS帯4			CSV_COL名：	親和力：CS帯4	
        public string 親和力CS帯4 { get; set; }
        //	親和力CS帯5			CSV_COL名：	親和力：CS帯5	
        public string 親和力CS帯5 { get; set; }
        //	協働力CS帯1			CSV_COL名：	協働力：CS帯1	
        public string 協働力CS帯1 { get; set; }
        //	協働力CS帯2			CSV_COL名：	協働力：CS帯2	
        public string 協働力CS帯2 { get; set; }
        //	協働力CS帯3			CSV_COL名：	協働力：CS帯3	
        public string 協働力CS帯3 { get; set; }
        //	協働力CS帯4			CSV_COL名：	協働力：CS帯4	
        public string 協働力CS帯4 { get; set; }
        //	協働力CS帯5			CSV_COL名：	協働力：CS帯5	
        public string 協働力CS帯5 { get; set; }
        //	統率力CS帯1			CSV_COL名：	統率力：CS帯1	
        public string 統率力CS帯1 { get; set; }
        //	統率力CS帯2			CSV_COL名：	統率力：CS帯2	
        public string 統率力CS帯2 { get; set; }
        //	統率力CS帯3			CSV_COL名：	統率力：CS帯3	
        public string 統率力CS帯3 { get; set; }
        //	統率力CS帯4			CSV_COL名：	統率力：CS帯4	
        public string 統率力CS帯4 { get; set; }
        //	統率力CS帯5			CSV_COL名：	統率力：CS帯5	
        public string 統率力CS帯5 { get; set; }
        //	感情制御力CS帯1			CSV_COL名：	感情制御力：CS帯1	
        public string 感情制御力CS帯1 { get; set; }
        //	感情制御力CS帯2			CSV_COL名：	感情制御力：CS帯2	
        public string 感情制御力CS帯2 { get; set; }
        //	感情制御力CS帯3			CSV_COL名：	感情制御力：CS帯3	
        public string 感情制御力CS帯3 { get; set; }
        //	感情制御力CS帯4			CSV_COL名：	感情制御力：CS帯4	
        public string 感情制御力CS帯4 { get; set; }
        //	感情制御力CS帯5			CSV_COL名：	感情制御力：CS帯5	
        public string 感情制御力CS帯5 { get; set; }
        //	自信創出力CS帯1			CSV_COL名：	自信創出力：CS帯1	
        public string 自信創出力CS帯1 { get; set; }
        //	自信創出力CS帯2			CSV_COL名：	自信創出力：CS帯2	
        public string 自信創出力CS帯2 { get; set; }
        //	自信創出力CS帯3			CSV_COL名：	自信創出力：CS帯3	
        public string 自信創出力CS帯3 { get; set; }
        //	自信創出力CS帯4			CSV_COL名：	自信創出力：CS帯4	
        public string 自信創出力CS帯4 { get; set; }
        //	自信創出力CS帯5			CSV_COL名：	自信創出力：CS帯5	
        public string 自信創出力CS帯5 { get; set; }
        //	行動持続力CS帯1			CSV_COL名：	行動持続力：CS帯1	
        public string 行動持続力CS帯1 { get; set; }
        //	行動持続力CS帯2			CSV_COL名：	行動持続力：CS帯2	
        public string 行動持続力CS帯2 { get; set; }
        //	行動持続力CS帯3			CSV_COL名：	行動持続力：CS帯3	
        public string 行動持続力CS帯3 { get; set; }
        //	行動持続力CS帯4			CSV_COL名：	行動持続力：CS帯4	
        public string 行動持続力CS帯4 { get; set; }
        //	行動持続力CS帯5			CSV_COL名：	行動持続力：CS帯5	
        public string 行動持続力CS帯5 { get; set; }
        //	課題発見力CS帯1			CSV_COL名：	課題発見力：CS帯1	
        public string 課題発見力CS帯1 { get; set; }
        //	課題発見力CS帯2			CSV_COL名：	課題発見力：CS帯2	
        public string 課題発見力CS帯2 { get; set; }
        //	課題発見力CS帯3			CSV_COL名：	課題発見力：CS帯3	
        public string 課題発見力CS帯3 { get; set; }
        //	課題発見力CS帯4			CSV_COL名：	課題発見力：CS帯4	
        public string 課題発見力CS帯4 { get; set; }
        //	課題発見力CS帯5			CSV_COL名：	課題発見力：CS帯5	
        public string 課題発見力CS帯5 { get; set; }
        //	計画立案力CS帯1			CSV_COL名：	計画立案力：CS帯1	
        public string 計画立案力CS帯1 { get; set; }
        //	計画立案力CS帯2			CSV_COL名：	計画立案力：CS帯2	
        public string 計画立案力CS帯2 { get; set; }
        //	計画立案力CS帯3			CSV_COL名：	計画立案力：CS帯3	
        public string 計画立案力CS帯3 { get; set; }
        //	計画立案力CS帯4			CSV_COL名：	計画立案力：CS帯4	
        public string 計画立案力CS帯4 { get; set; }
        //	計画立案力CS帯5			CSV_COL名：	計画立案力：CS帯5	
        public string 計画立案力CS帯5 { get; set; }
        //	実践力CS帯1			CSV_COL名：	実践力：CS帯1	
        public string 実践力CS帯1 { get; set; }
        //	実践力CS帯2			CSV_COL名：	実践力：CS帯2	
        public string 実践力CS帯2 { get; set; }
        //	実践力CS帯3			CSV_COL名：	実践力：CS帯3	
        public string 実践力CS帯3 { get; set; }
        //	実践力CS帯4			CSV_COL名：	実践力：CS帯4	
        public string 実践力CS帯4 { get; set; }
        //	実践力CS帯5			CSV_COL名：	実践力：CS帯5	
        public string 実践力CS帯5 { get; set; }
        //	親しみやすさレベル			CSV_COL名：	親しみやすさ：レベル	
        public string 親しみやすさレベル { get; set; }
        //	気配りレベル			CSV_COL名：	気配り：レベル	
        public string 気配りレベル { get; set; }
        //	対人興味共感受容レベル			CSV_COL名：	対人興味／共感・受容：レベル	
        public string 対人興味共感受容レベル { get; set; }
        //	多様性理解レベル			CSV_COL名：	多様性理解：レベル	
        public string 多様性理解レベル { get; set; }
        //	人脈形成レベル			CSV_COL名：	人脈形成：レベル	
        public string 人脈形成レベル { get; set; }
        //	信頼構築レベル			CSV_COL名：	信頼構築：レベル	
        public string 信頼構築レベル { get; set; }
        //	役割理解連携行動レベル			CSV_COL名：	役割理解・連携行動：レベル	
        public string 役割理解連携行動レベル { get; set; }
        //	情報共有レベル			CSV_COL名：	情報共有：レベル	
        public string 情報共有レベル { get; set; }
        //	相互支援レベル			CSV_COL名：	相互支援：レベル	
        public string 相互支援レベル { get; set; }
        //	相談指導他者の動機づけレベル			CSV_COL名：	相談・指導・他者の動機づけ：レベル	
        public string 相談指導他者の動機づけレベル { get; set; }
        //	話しあうレベル			CSV_COL名：	話しあう：レベル	
        public string 話しあうレベル { get; set; }
        //	意見を主張するレベル			CSV_COL名：	意見を主張する：レベル	
        public string 意見を主張するレベル { get; set; }
        //	建設的創造的な討議レベル			CSV_COL名：	建設的・創造的な討議：レベル	
        public string 建設的創造的な討議レベル { get; set; }
        //	意見の調整交渉説得レベル			CSV_COL名：	意見の調整、交渉、説得：レベル	
        public string 意見の調整交渉説得レベル { get; set; }
        //	セルフアウェアネスレベル			CSV_COL名：	セルフアウェアネス：レベル	
        public string セルフアウェアネスレベル { get; set; }
        //	ストレスコーピングレベル			CSV_COL名：	ストレスコーピング：レベル	
        public string ストレスコーピングレベル { get; set; }
        //	ストレスマネジメントレベル			CSV_COL名：	ストレスマネジメント：レベル	
        public string ストレスマネジメントレベル { get; set; }
        //	独自性理解レベル			CSV_COL名：	独自性理解：レベル	
        public string 独自性理解レベル { get; set; }
        //	自己効力感楽観性レベル			CSV_COL名：	自己効力感／楽観性：レベル	
        public string 自己効力感楽観性レベル { get; set; }
        //	学習視点機会による自己変革レベル			CSV_COL名：	学習視点・機会による自己変革：レベル	
        public string 学習視点機会による自己変革レベル { get; set; }
        //	主体的行動レベル			CSV_COL名：	主体的行動：レベル	
        public string 主体的行動レベル { get; set; }
        //	完遂レベル			CSV_COL名：	完遂：レベル	
        public string 完遂レベル { get; set; }
        //	良い行動の習慣化レベル			CSV_COL名：	良い行動の習慣化：レベル	
        public string 良い行動の習慣化レベル { get; set; }
        //	情報収集レベル			CSV_COL名：	情報収集：レベル	
        public string 情報収集レベル { get; set; }
        //	本質理解レベル			CSV_COL名：	本質理解：レベル	
        public string 本質理解レベル { get; set; }
        //	原因追究レベル			CSV_COL名：	原因追究：レベル	
        public string 原因追究レベル { get; set; }
        //	目標設定レベル			CSV_COL名：	目標設定：レベル	
        public string 目標設定レベル { get; set; }
        //	シナリオ構築レベル			CSV_COL名：	シナリオ構築：レベル	
        public string シナリオ構築レベル { get; set; }
        //	計画評価レベル			CSV_COL名：	計画評価：レベル	
        public string 計画評価レベル { get; set; }
        //	リスク分析レベル			CSV_COL名：	リスク分析：レベル	
        public string リスク分析レベル { get; set; }
        //	実践行動レベル			CSV_COL名：	実践行動：レベル	
        public string 実践行動レベル { get; set; }
        //	修正調整レベル			CSV_COL名：	修正／調整：レベル	
        public string 修正調整レベル { get; set; }
        //	検証改善レベル			CSV_COL名：	検証／改善：レベル	
        public string 検証改善レベル { get; set; }
        //	親しみやすさ強化ポイント			CSV_COL名：	親しみやすさ：強化ポイント	
        public string 親しみやすさ強化ポイント { get; set; }
        //	気配り強化ポイント			CSV_COL名：	気配り：強化ポイント	
        public string 気配り強化ポイント { get; set; }
        //	対人興味共感受容強化ポイント			CSV_COL名：	対人興味／共感・受容：強化ポイント	
        public string 対人興味共感受容強化ポイント { get; set; }
        //	多様性理解強化ポイント			CSV_COL名：	多様性理解：強化ポイント	
        public string 多様性理解強化ポイント { get; set; }
        //	人脈形成強化ポイント			CSV_COL名：	人脈形成：強化ポイント	
        public string 人脈形成強化ポイント { get; set; }
        //	信頼構築強化ポイント			CSV_COL名：	信頼構築：強化ポイント	
        public string 信頼構築強化ポイント { get; set; }
        //	役割理解連携行動強化ポイント			CSV_COL名：	役割理解・連携行動：強化ポイント	
        public string 役割理解連携行動強化ポイント { get; set; }
        //	情報共有強化ポイント			CSV_COL名：	情報共有：強化ポイント	
        public string 情報共有強化ポイント { get; set; }
        //	相互支援強化ポイント			CSV_COL名：	相互支援：強化ポイント	
        public string 相互支援強化ポイント { get; set; }
        //	相談指導他者の動機づけ強化ポイント			CSV_COL名：	相談・指導・他者の動機づけ：強化ポイント	
        public string 相談指導他者の動機づけ強化ポイント { get; set; }
        //	話しあう強化ポイント			CSV_COL名：	話しあう：強化ポイント	
        public string 話しあう強化ポイント { get; set; }
        //	意見を主張する強化ポイント			CSV_COL名：	意見を主張する：強化ポイント	
        public string 意見を主張する強化ポイント { get; set; }
        //	建設的創造的な討議強化ポイント			CSV_COL名：	建設的・創造的な討議：強化ポイント	
        public string 建設的創造的な討議強化ポイント { get; set; }
        //	意見の調整交渉説得強化ポイント			CSV_COL名：	意見の調整、交渉、説得：強化ポイント	
        public string 意見の調整交渉説得強化ポイント { get; set; }
        //	セルフアウェアネス強化ポイント			CSV_COL名：	セルフアウェアネス：強化ポイント	
        public string セルフアウェアネス強化ポイント { get; set; }
        //	ストレスコーピング強化ポイント			CSV_COL名：	ストレスコーピング：強化ポイント	
        public string ストレスコーピング強化ポイント { get; set; }
        //	ストレスマネジメント強化ポイント			CSV_COL名：	ストレスマネジメント：強化ポイント	
        public string ストレスマネジメント強化ポイント { get; set; }
        //	独自性理解強化ポイント			CSV_COL名：	独自性理解：強化ポイント	
        public string 独自性理解強化ポイント { get; set; }
        //	自己効力感楽観性強化ポイント			CSV_COL名：	自己効力感／楽観性：強化ポイント	
        public string 自己効力感楽観性強化ポイント { get; set; }
        //	学習視点機会による自己変革強化ポイント			CSV_COL名：	学習視点・機会による自己変革：強化ポイント	
        public string 学習視点機会による自己変革強化ポイント { get; set; }
        //	主体的行動強化ポイント			CSV_COL名：	主体的行動：強化ポイント	
        public string 主体的行動強化ポイント { get; set; }
        //	完遂強化ポイント			CSV_COL名：	完遂：強化ポイント	
        public string 完遂強化ポイント { get; set; }
        //	良い行動の習慣化強化ポイント			CSV_COL名：	良い行動の習慣化：強化ポイント	
        public string 良い行動の習慣化強化ポイント { get; set; }
        //	情報収集強化ポイント			CSV_COL名：	情報収集：強化ポイント	
        public string 情報収集強化ポイント { get; set; }
        //	本質理解強化ポイント			CSV_COL名：	本質理解：強化ポイント	
        public string 本質理解強化ポイント { get; set; }
        //	原因追究強化ポイント			CSV_COL名：	原因追究：強化ポイント	
        public string 原因追究強化ポイント { get; set; }
        //	目標設定強化ポイント			CSV_COL名：	目標設定：強化ポイント	
        public string 目標設定強化ポイント { get; set; }
        //	シナリオ構築強化ポイント			CSV_COL名：	シナリオ構築：強化ポイント	
        public string シナリオ構築強化ポイント { get; set; }
        //	計画評価強化ポイント			CSV_COL名：	計画評価：強化ポイント	
        public string 計画評価強化ポイント { get; set; }
        //	リスク分析強化ポイント			CSV_COL名：	リスク分析：強化ポイント	
        public string リスク分析強化ポイント { get; set; }
        //	実践行動強化ポイント			CSV_COL名：	実践行動：強化ポイント	
        public string 実践行動強化ポイント { get; set; }
        //	修正調整強化ポイント			CSV_COL名：	修正／調整：強化ポイント	
        public string 修正調整強化ポイント { get; set; }
        //	検証改善強化ポイント			CSV_COL名：	検証／改善：強化ポイント	
        public string 検証改善強化ポイント { get; set; }
        //	親しみやすさCS平均			CSV_COL名：	親しみやすさ：CS平均	
        public string 親しみやすさCS平均 { get; set; }
        //	気配りCS平均			CSV_COL名：	気配り：CS平均	
        public string 気配りCS平均 { get; set; }
        //	対人興味共感受容CS平均			CSV_COL名：	対人興味／共感・受容：CS平均	
        public string 対人興味共感受容CS平均 { get; set; }
        //	多様性理解CS平均			CSV_COL名：	多様性理解：CS平均	
        public string 多様性理解CS平均 { get; set; }
        //	人脈形成CS平均			CSV_COL名：	人脈形成：CS平均	
        public string 人脈形成CS平均 { get; set; }
        //	信頼構築CS平均			CSV_COL名：	信頼構築：CS平均	
        public string 信頼構築CS平均 { get; set; }
        //	役割理解連携行動CS平均			CSV_COL名：	役割理解・連携行動：CS平均	
        public string 役割理解連携行動CS平均 { get; set; }
        //	情報共有CS平均			CSV_COL名：	情報共有：CS平均	
        public string 情報共有CS平均 { get; set; }
        //	相互支援CS平均			CSV_COL名：	相互支援：CS平均	
        public string 相互支援CS平均 { get; set; }
        //	相談指導他者の動機づけCS平均			CSV_COL名：	相談・指導・他者の動機づけ：CS平均	
        public string 相談指導他者の動機づけCS平均 { get; set; }
        //	話しあうCS平均			CSV_COL名：	話しあう：CS平均	
        public string 話しあうCS平均 { get; set; }
        //	意見を主張するCS平均			CSV_COL名：	意見を主張する：CS平均	
        public string 意見を主張するCS平均 { get; set; }
        //	建設的創造的な討議CS平均			CSV_COL名：	建設的・創造的な討議：CS平均	
        public string 建設的創造的な討議CS平均 { get; set; }
        //	意見の調整交渉説得CS平均			CSV_COL名：	意見の調整、交渉、説得：CS平均	
        public string 意見の調整交渉説得CS平均 { get; set; }
        //	セルフアウェアネスCS平均			CSV_COL名：	セルフアウェアネス：CS平均	
        public string セルフアウェアネスCS平均 { get; set; }
        //	ストレスコーピングCS平均			CSV_COL名：	ストレスコーピング：CS平均	
        public string ストレスコーピングCS平均 { get; set; }
        //	ストレスマネジメントCS平均			CSV_COL名：	ストレスマネジメント：CS平均	
        public string ストレスマネジメントCS平均 { get; set; }
        //	独自性理解CS平均			CSV_COL名：	独自性理解：CS平均	
        public string 独自性理解CS平均 { get; set; }
        //	自己効力感楽観性CS平均			CSV_COL名：	自己効力感／楽観性：CS平均	
        public string 自己効力感楽観性CS平均 { get; set; }
        //	学習視点機会による自己変革CS平均			CSV_COL名：	学習視点・機会による自己変革：CS平均	
        public string 学習視点機会による自己変革CS平均 { get; set; }
        //	主体的行動CS平均			CSV_COL名：	主体的行動：CS平均	
        public string 主体的行動CS平均 { get; set; }
        //	完遂CS平均			CSV_COL名：	完遂：CS平均	
        public string 完遂CS平均 { get; set; }
        //	良い行動の習慣化CS平均			CSV_COL名：	良い行動の習慣化：CS平均	
        public string 良い行動の習慣化CS平均 { get; set; }
        //	情報収集CS平均			CSV_COL名：	情報収集：CS平均	
        public string 情報収集CS平均 { get; set; }
        //	本質理解CS平均			CSV_COL名：	本質理解：CS平均	
        public string 本質理解CS平均 { get; set; }
        //	原因追究CS平均			CSV_COL名：	原因追究：CS平均	
        public string 原因追究CS平均 { get; set; }
        //	目標設定CS平均			CSV_COL名：	目標設定：CS平均	
        public string 目標設定CS平均 { get; set; }
        //	シナリオ構築CS平均			CSV_COL名：	シナリオ構築：CS平均	
        public string シナリオ構築CS平均 { get; set; }
        //	計画評価CS平均			CSV_COL名：	計画評価：CS平均	
        public string 計画評価CS平均 { get; set; }
        //	リスク分析CS平均			CSV_COL名：	リスク分析：CS平均	
        public string リスク分析CS平均 { get; set; }
        //	実践行動CS平均			CSV_COL名：	実践行動：CS平均	
        public string 実践行動CS平均 { get; set; }
        //	修正調整CS平均			CSV_COL名：	修正／調整：CS平均	
        public string 修正調整CS平均 { get; set; }
        //	検証改善CS平均			CSV_COL名：	検証／改善：CS平均	
        public string 検証改善CS平均 { get; set; }
        //	親しみやすさCS帯1			CSV_COL名：	親しみやすさ：CS帯1	
        public string 親しみやすさCS帯1 { get; set; }
        //	親しみやすさCS帯2			CSV_COL名：	親しみやすさ：CS帯2	
        public string 親しみやすさCS帯2 { get; set; }
        //	親しみやすさCS帯3			CSV_COL名：	親しみやすさ：CS帯3	
        public string 親しみやすさCS帯3 { get; set; }
        //	親しみやすさCS帯4			CSV_COL名：	親しみやすさ：CS帯4	
        public string 親しみやすさCS帯4 { get; set; }
        //	親しみやすさCS帯5			CSV_COL名：	親しみやすさ：CS帯5	
        public string 親しみやすさCS帯5 { get; set; }
        //	気配りCS帯1			CSV_COL名：	気配り：CS帯1	
        public string 気配りCS帯1 { get; set; }
        //	気配りCS帯2			CSV_COL名：	気配り：CS帯2	
        public string 気配りCS帯2 { get; set; }
        //	気配りCS帯3			CSV_COL名：	気配り：CS帯3	
        public string 気配りCS帯3 { get; set; }
        //	気配りCS帯4			CSV_COL名：	気配り：CS帯4	
        public string 気配りCS帯4 { get; set; }
        //	気配りCS帯5			CSV_COL名：	気配り：CS帯5	
        public string 気配りCS帯5 { get; set; }
        //	対人興味共感受容CS帯1			CSV_COL名：	対人興味／共感・受容：CS帯1	
        public string 対人興味共感受容CS帯1 { get; set; }
        //	対人興味共感受容CS帯2			CSV_COL名：	対人興味／共感・受容：CS帯2	
        public string 対人興味共感受容CS帯2 { get; set; }
        //	対人興味共感受容CS帯3			CSV_COL名：	対人興味／共感・受容：CS帯3	
        public string 対人興味共感受容CS帯3 { get; set; }
        //	対人興味共感受容CS帯4			CSV_COL名：	対人興味／共感・受容：CS帯4	
        public string 対人興味共感受容CS帯4 { get; set; }
        //	対人興味共感受容CS帯5			CSV_COL名：	対人興味／共感・受容：CS帯5	
        public string 対人興味共感受容CS帯5 { get; set; }
        //	多様性理解CS帯1			CSV_COL名：	多様性理解：CS帯1	
        public string 多様性理解CS帯1 { get; set; }
        //	多様性理解CS帯2			CSV_COL名：	多様性理解：CS帯2	
        public string 多様性理解CS帯2 { get; set; }
        //	多様性理解CS帯3			CSV_COL名：	多様性理解：CS帯3	
        public string 多様性理解CS帯3 { get; set; }
        //	多様性理解CS帯4			CSV_COL名：	多様性理解：CS帯4	
        public string 多様性理解CS帯4 { get; set; }
        //	多様性理解CS帯5			CSV_COL名：	多様性理解：CS帯5	
        public string 多様性理解CS帯5 { get; set; }
        //	人脈形成CS帯1			CSV_COL名：	人脈形成：CS帯1	
        public string 人脈形成CS帯1 { get; set; }
        //	人脈形成CS帯2			CSV_COL名：	人脈形成：CS帯2	
        public string 人脈形成CS帯2 { get; set; }
        //	人脈形成CS帯3			CSV_COL名：	人脈形成：CS帯3	
        public string 人脈形成CS帯3 { get; set; }
        //	人脈形成CS帯4			CSV_COL名：	人脈形成：CS帯4	
        public string 人脈形成CS帯4 { get; set; }
        //	人脈形成CS帯5			CSV_COL名：	人脈形成：CS帯5	
        public string 人脈形成CS帯5 { get; set; }
        //	信頼構築CS帯1			CSV_COL名：	信頼構築：CS帯1	
        public string 信頼構築CS帯1 { get; set; }
        //	信頼構築CS帯2			CSV_COL名：	信頼構築：CS帯2	
        public string 信頼構築CS帯2 { get; set; }
        //	信頼構築CS帯3			CSV_COL名：	信頼構築：CS帯3	
        public string 信頼構築CS帯3 { get; set; }
        //	信頼構築CS帯4			CSV_COL名：	信頼構築：CS帯4	
        public string 信頼構築CS帯4 { get; set; }
        //	信頼構築CS帯5			CSV_COL名：	信頼構築：CS帯5	
        public string 信頼構築CS帯5 { get; set; }
        //	役割理解連携行動CS帯1			CSV_COL名：	役割理解・連携行動：CS帯1	
        public string 役割理解連携行動CS帯1 { get; set; }
        //	役割理解連携行動CS帯2			CSV_COL名：	役割理解・連携行動：CS帯2	
        public string 役割理解連携行動CS帯2 { get; set; }
        //	役割理解連携行動CS帯3			CSV_COL名：	役割理解・連携行動：CS帯3	
        public string 役割理解連携行動CS帯3 { get; set; }
        //	役割理解連携行動CS帯4			CSV_COL名：	役割理解・連携行動：CS帯4	
        public string 役割理解連携行動CS帯4 { get; set; }
        //	役割理解連携行動CS帯5			CSV_COL名：	役割理解・連携行動：CS帯5	
        public string 役割理解連携行動CS帯5 { get; set; }
        //	情報共有CS帯1			CSV_COL名：	情報共有：CS帯1	
        public string 情報共有CS帯1 { get; set; }
        //	情報共有CS帯2			CSV_COL名：	情報共有：CS帯2	
        public string 情報共有CS帯2 { get; set; }
        //	情報共有CS帯3			CSV_COL名：	情報共有：CS帯3	
        public string 情報共有CS帯3 { get; set; }
        //	情報共有CS帯4			CSV_COL名：	情報共有：CS帯4	
        public string 情報共有CS帯4 { get; set; }
        //	情報共有CS帯5			CSV_COL名：	情報共有：CS帯5	
        public string 情報共有CS帯5 { get; set; }
        //	相互支援CS帯1			CSV_COL名：	相互支援：CS帯1	
        public string 相互支援CS帯1 { get; set; }
        //	相互支援CS帯2			CSV_COL名：	相互支援：CS帯2	
        public string 相互支援CS帯2 { get; set; }
        //	相互支援CS帯3			CSV_COL名：	相互支援：CS帯3	
        public string 相互支援CS帯3 { get; set; }
        //	相互支援CS帯4			CSV_COL名：	相互支援：CS帯4	
        public string 相互支援CS帯4 { get; set; }
        //	相互支援CS帯5			CSV_COL名：	相互支援：CS帯5	
        public string 相互支援CS帯5 { get; set; }
        //	相談指導他者の動機づけCS帯1			CSV_COL名：	相談・指導・他者の動機づけ：CS帯1	
        public string 相談指導他者の動機づけCS帯1 { get; set; }
        //	相談指導他者の動機づけCS帯2			CSV_COL名：	相談・指導・他者の動機づけ：CS帯2	
        public string 相談指導他者の動機づけCS帯2 { get; set; }
        //	相談指導他者の動機づけCS帯3			CSV_COL名：	相談・指導・他者の動機づけ：CS帯3	
        public string 相談指導他者の動機づけCS帯3 { get; set; }
        //	相談指導他者の動機づけCS帯4			CSV_COL名：	相談・指導・他者の動機づけ：CS帯4	
        public string 相談指導他者の動機づけCS帯4 { get; set; }
        //	相談指導他者の動機づけCS帯5			CSV_COL名：	相談・指導・他者の動機づけ：CS帯5	
        public string 相談指導他者の動機づけCS帯5 { get; set; }
        //	話しあうCS帯1			CSV_COL名：	話しあう：CS帯1	
        public string 話しあうCS帯1 { get; set; }
        //	話しあうCS帯2			CSV_COL名：	話しあう：CS帯2	
        public string 話しあうCS帯2 { get; set; }
        //	話しあうCS帯3			CSV_COL名：	話しあう：CS帯3	
        public string 話しあうCS帯3 { get; set; }
        //	話しあうCS帯4			CSV_COL名：	話しあう：CS帯4	
        public string 話しあうCS帯4 { get; set; }
        //	話しあうCS帯5			CSV_COL名：	話しあう：CS帯5	
        public string 話しあうCS帯5 { get; set; }
        //	意見を主張するCS帯1			CSV_COL名：	意見を主張する：CS帯1	
        public string 意見を主張するCS帯1 { get; set; }
        //	意見を主張するCS帯2			CSV_COL名：	意見を主張する：CS帯2	
        public string 意見を主張するCS帯2 { get; set; }
        //	意見を主張するCS帯3			CSV_COL名：	意見を主張する：CS帯3	
        public string 意見を主張するCS帯3 { get; set; }
        //	意見を主張するCS帯4			CSV_COL名：	意見を主張する：CS帯4	
        public string 意見を主張するCS帯4 { get; set; }
        //	意見を主張するCS帯5			CSV_COL名：	意見を主張する：CS帯5	
        public string 意見を主張するCS帯5 { get; set; }
        //	建設的創造的な討議CS帯1			CSV_COL名：	建設的・創造的な討議：CS帯1	
        public string 建設的創造的な討議CS帯1 { get; set; }
        //	建設的創造的な討議CS帯2			CSV_COL名：	建設的・創造的な討議：CS帯2	
        public string 建設的創造的な討議CS帯2 { get; set; }
        //	建設的創造的な討議CS帯3			CSV_COL名：	建設的・創造的な討議：CS帯3	
        public string 建設的創造的な討議CS帯3 { get; set; }
        //	建設的創造的な討議CS帯4			CSV_COL名：	建設的・創造的な討議：CS帯4	
        public string 建設的創造的な討議CS帯4 { get; set; }
        //	建設的創造的な討議CS帯5			CSV_COL名：	建設的・創造的な討議：CS帯5	
        public string 建設的創造的な討議CS帯5 { get; set; }
        //	意見の調整交渉説得CS帯1			CSV_COL名：	意見の調整、交渉、説得：CS帯1	
        public string 意見の調整交渉説得CS帯1 { get; set; }
        //	意見の調整交渉説得CS帯2			CSV_COL名：	意見の調整、交渉、説得：CS帯2	
        public string 意見の調整交渉説得CS帯2 { get; set; }
        //	意見の調整交渉説得CS帯3			CSV_COL名：	意見の調整、交渉、説得：CS帯3	
        public string 意見の調整交渉説得CS帯3 { get; set; }
        //	意見の調整交渉説得CS帯4			CSV_COL名：	意見の調整、交渉、説得：CS帯4	
        public string 意見の調整交渉説得CS帯4 { get; set; }
        //	意見の調整交渉説得CS帯5			CSV_COL名：	意見の調整、交渉、説得：CS帯5	
        public string 意見の調整交渉説得CS帯5 { get; set; }
        //	セルフアウェアネスCS帯1			CSV_COL名：	セルフアウェアネス：CS帯1	
        public string セルフアウェアネスCS帯1 { get; set; }
        //	セルフアウェアネスCS帯2			CSV_COL名：	セルフアウェアネス：CS帯2	
        public string セルフアウェアネスCS帯2 { get; set; }
        //	セルフアウェアネスCS帯3			CSV_COL名：	セルフアウェアネス：CS帯3	
        public string セルフアウェアネスCS帯3 { get; set; }
        //	セルフアウェアネスCS帯4			CSV_COL名：	セルフアウェアネス：CS帯4	
        public string セルフアウェアネスCS帯4 { get; set; }
        //	セルフアウェアネスCS帯5			CSV_COL名：	セルフアウェアネス：CS帯5	
        public string セルフアウェアネスCS帯5 { get; set; }
        //	ストレスコーピングCS帯1			CSV_COL名：	ストレスコーピング：CS帯1	
        public string ストレスコーピングCS帯1 { get; set; }
        //	ストレスコーピングCS帯2			CSV_COL名：	ストレスコーピング：CS帯2	
        public string ストレスコーピングCS帯2 { get; set; }
        //	ストレスコーピングCS帯3			CSV_COL名：	ストレスコーピング：CS帯3	
        public string ストレスコーピングCS帯3 { get; set; }
        //	ストレスコーピングCS帯4			CSV_COL名：	ストレスコーピング：CS帯4	
        public string ストレスコーピングCS帯4 { get; set; }
        //	ストレスコーピングCS帯5			CSV_COL名：	ストレスコーピング：CS帯5	
        public string ストレスコーピングCS帯5 { get; set; }
        //	ストレスマネジメントCS帯1			CSV_COL名：	ストレスマネジメント：CS帯1	
        public string ストレスマネジメントCS帯1 { get; set; }
        //	ストレスマネジメントCS帯2			CSV_COL名：	ストレスマネジメント：CS帯2	
        public string ストレスマネジメントCS帯2 { get; set; }
        //	ストレスマネジメントCS帯3			CSV_COL名：	ストレスマネジメント：CS帯3	
        public string ストレスマネジメントCS帯3 { get; set; }
        //	ストレスマネジメントCS帯4			CSV_COL名：	ストレスマネジメント：CS帯4	
        public string ストレスマネジメントCS帯4 { get; set; }
        //	ストレスマネジメントCS帯5			CSV_COL名：	ストレスマネジメント：CS帯5	
        public string ストレスマネジメントCS帯5 { get; set; }
        //	独自性理解CS帯1			CSV_COL名：	独自性理解：CS帯1	
        public string 独自性理解CS帯1 { get; set; }
        //	独自性理解CS帯2			CSV_COL名：	独自性理解：CS帯2	
        public string 独自性理解CS帯2 { get; set; }
        //	独自性理解CS帯3			CSV_COL名：	独自性理解：CS帯3	
        public string 独自性理解CS帯3 { get; set; }
        //	独自性理解CS帯4			CSV_COL名：	独自性理解：CS帯4	
        public string 独自性理解CS帯4 { get; set; }
        //	独自性理解CS帯5			CSV_COL名：	独自性理解：CS帯5	
        public string 独自性理解CS帯5 { get; set; }
        //	自己効力感楽観性CS帯1			CSV_COL名：	自己効力感／楽観性：CS帯1	
        public string 自己効力感楽観性CS帯1 { get; set; }
        //	自己効力感楽観性CS帯2			CSV_COL名：	自己効力感／楽観性：CS帯2	
        public string 自己効力感楽観性CS帯2 { get; set; }
        //	自己効力感楽観性CS帯3			CSV_COL名：	自己効力感／楽観性：CS帯3	
        public string 自己効力感楽観性CS帯3 { get; set; }
        //	自己効力感楽観性CS帯4			CSV_COL名：	自己効力感／楽観性：CS帯4	
        public string 自己効力感楽観性CS帯4 { get; set; }
        //	自己効力感楽観性CS帯5			CSV_COL名：	自己効力感／楽観性：CS帯5	
        public string 自己効力感楽観性CS帯5 { get; set; }
        //	学習視点機会による自己変革CS帯1			CSV_COL名：	学習視点・機会による自己変革：CS帯1	
        public string 学習視点機会による自己変革CS帯1 { get; set; }
        //	学習視点機会による自己変革CS帯2			CSV_COL名：	学習視点・機会による自己変革：CS帯2	
        public string 学習視点機会による自己変革CS帯2 { get; set; }
        //	学習視点機会による自己変革CS帯3			CSV_COL名：	学習視点・機会による自己変革：CS帯3	
        public string 学習視点機会による自己変革CS帯3 { get; set; }
        //	学習視点機会による自己変革CS帯4			CSV_COL名：	学習視点・機会による自己変革：CS帯4	
        public string 学習視点機会による自己変革CS帯4 { get; set; }
        //	学習視点機会による自己変革CS帯5			CSV_COL名：	学習視点・機会による自己変革：CS帯5	
        public string 学習視点機会による自己変革CS帯5 { get; set; }
        //	主体的行動CS帯1			CSV_COL名：	主体的行動：CS帯1	
        public string 主体的行動CS帯1 { get; set; }
        //	主体的行動CS帯2			CSV_COL名：	主体的行動：CS帯2	
        public string 主体的行動CS帯2 { get; set; }
        //	主体的行動CS帯3			CSV_COL名：	主体的行動：CS帯3	
        public string 主体的行動CS帯3 { get; set; }
        //	主体的行動CS帯4			CSV_COL名：	主体的行動：CS帯4	
        public string 主体的行動CS帯4 { get; set; }
        //	主体的行動CS帯5			CSV_COL名：	主体的行動：CS帯5	
        public string 主体的行動CS帯5 { get; set; }
        //	完遂CS帯1			CSV_COL名：	完遂：CS帯1	
        public string 完遂CS帯1 { get; set; }
        //	完遂CS帯2			CSV_COL名：	完遂：CS帯2	
        public string 完遂CS帯2 { get; set; }
        //	完遂CS帯3			CSV_COL名：	完遂：CS帯3	
        public string 完遂CS帯3 { get; set; }
        //	完遂CS帯4			CSV_COL名：	完遂：CS帯4	
        public string 完遂CS帯4 { get; set; }
        //	完遂CS帯5			CSV_COL名：	完遂：CS帯5	
        public string 完遂CS帯5 { get; set; }
        //	良い行動の習慣化CS帯1			CSV_COL名：	良い行動の習慣化：CS帯1	
        public string 良い行動の習慣化CS帯1 { get; set; }
        //	良い行動の習慣化CS帯2			CSV_COL名：	良い行動の習慣化：CS帯2	
        public string 良い行動の習慣化CS帯2 { get; set; }
        //	良い行動の習慣化CS帯3			CSV_COL名：	良い行動の習慣化：CS帯3	
        public string 良い行動の習慣化CS帯3 { get; set; }
        //	良い行動の習慣化CS帯4			CSV_COL名：	良い行動の習慣化：CS帯4	
        public string 良い行動の習慣化CS帯4 { get; set; }
        //	良い行動の習慣化CS帯5			CSV_COL名：	良い行動の習慣化：CS帯5	
        public string 良い行動の習慣化CS帯5 { get; set; }
        //	情報収集CS帯1			CSV_COL名：	情報収集：CS帯1	
        public string 情報収集CS帯1 { get; set; }
        //	情報収集CS帯2			CSV_COL名：	情報収集：CS帯2	
        public string 情報収集CS帯2 { get; set; }
        //	情報収集CS帯3			CSV_COL名：	情報収集：CS帯3	
        public string 情報収集CS帯3 { get; set; }
        //	情報収集CS帯4			CSV_COL名：	情報収集：CS帯4	
        public string 情報収集CS帯4 { get; set; }
        //	情報収集CS帯5			CSV_COL名：	情報収集：CS帯5	
        public string 情報収集CS帯5 { get; set; }
        //	本質理解CS帯1			CSV_COL名：	本質理解：CS帯1	
        public string 本質理解CS帯1 { get; set; }
        //	本質理解CS帯2			CSV_COL名：	本質理解：CS帯2	
        public string 本質理解CS帯2 { get; set; }
        //	本質理解CS帯3			CSV_COL名：	本質理解：CS帯3	
        public string 本質理解CS帯3 { get; set; }
        //	本質理解CS帯4			CSV_COL名：	本質理解：CS帯4	
        public string 本質理解CS帯4 { get; set; }
        //	本質理解CS帯5			CSV_COL名：	本質理解：CS帯5	
        public string 本質理解CS帯5 { get; set; }
        //	原因追究CS帯1			CSV_COL名：	原因追究：CS帯1	
        public string 原因追究CS帯1 { get; set; }
        //	原因追究CS帯2			CSV_COL名：	原因追究：CS帯2	
        public string 原因追究CS帯2 { get; set; }
        //	原因追究CS帯3			CSV_COL名：	原因追究：CS帯3	
        public string 原因追究CS帯3 { get; set; }
        //	原因追究CS帯4			CSV_COL名：	原因追究：CS帯4	
        public string 原因追究CS帯4 { get; set; }
        //	原因追究CS帯5			CSV_COL名：	原因追究：CS帯5	
        public string 原因追究CS帯5 { get; set; }
        //	目標設定CS帯1			CSV_COL名：	目標設定：CS帯1	
        public string 目標設定CS帯1 { get; set; }
        //	目標設定CS帯2			CSV_COL名：	目標設定：CS帯2	
        public string 目標設定CS帯2 { get; set; }
        //	目標設定CS帯3			CSV_COL名：	目標設定：CS帯3	
        public string 目標設定CS帯3 { get; set; }
        //	目標設定CS帯4			CSV_COL名：	目標設定：CS帯4	
        public string 目標設定CS帯4 { get; set; }
        //	目標設定CS帯5			CSV_COL名：	目標設定：CS帯5	
        public string 目標設定CS帯5 { get; set; }
        //	シナリオ構築CS帯1			CSV_COL名：	シナリオ構築：CS帯1	
        public string シナリオ構築CS帯1 { get; set; }
        //	シナリオ構築CS帯2			CSV_COL名：	シナリオ構築：CS帯2	
        public string シナリオ構築CS帯2 { get; set; }
        //	シナリオ構築CS帯3			CSV_COL名：	シナリオ構築：CS帯3	
        public string シナリオ構築CS帯3 { get; set; }
        //	シナリオ構築CS帯4			CSV_COL名：	シナリオ構築：CS帯4	
        public string シナリオ構築CS帯4 { get; set; }
        //	シナリオ構築CS帯5			CSV_COL名：	シナリオ構築：CS帯5	
        public string シナリオ構築CS帯5 { get; set; }
        //	計画評価CS帯1			CSV_COL名：	計画評価：CS帯1	
        public string 計画評価CS帯1 { get; set; }
        //	計画評価CS帯2			CSV_COL名：	計画評価：CS帯2	
        public string 計画評価CS帯2 { get; set; }
        //	計画評価CS帯3			CSV_COL名：	計画評価：CS帯3	
        public string 計画評価CS帯3 { get; set; }
        //	計画評価CS帯4			CSV_COL名：	計画評価：CS帯4	
        public string 計画評価CS帯4 { get; set; }
        //	計画評価CS帯5			CSV_COL名：	計画評価：CS帯5	
        public string 計画評価CS帯5 { get; set; }
        //	リスク分析CS帯1			CSV_COL名：	リスク分析：CS帯1	
        public string リスク分析CS帯1 { get; set; }
        //	リスク分析CS帯2			CSV_COL名：	リスク分析：CS帯2	
        public string リスク分析CS帯2 { get; set; }
        //	リスク分析CS帯3			CSV_COL名：	リスク分析：CS帯3	
        public string リスク分析CS帯3 { get; set; }
        //	リスク分析CS帯4			CSV_COL名：	リスク分析：CS帯4	
        public string リスク分析CS帯4 { get; set; }
        //	リスク分析CS帯5			CSV_COL名：	リスク分析：CS帯5	
        public string リスク分析CS帯5 { get; set; }
        //	実践行動CS帯1			CSV_COL名：	実践行動：CS帯1	
        public string 実践行動CS帯1 { get; set; }
        //	実践行動CS帯2			CSV_COL名：	実践行動：CS帯2	
        public string 実践行動CS帯2 { get; set; }
        //	実践行動CS帯3			CSV_COL名：	実践行動：CS帯3	
        public string 実践行動CS帯3 { get; set; }
        //	実践行動CS帯4			CSV_COL名：	実践行動：CS帯4	
        public string 実践行動CS帯4 { get; set; }
        //	実践行動CS帯5			CSV_COL名：	実践行動：CS帯5	
        public string 実践行動CS帯5 { get; set; }
        //	修正調整CS帯1			CSV_COL名：	修正／調整：CS帯1	
        public string 修正調整CS帯1 { get; set; }
        //	修正調整CS帯2			CSV_COL名：	修正／調整：CS帯2	
        public string 修正調整CS帯2 { get; set; }
        //	修正調整CS帯3			CSV_COL名：	修正／調整：CS帯3	
        public string 修正調整CS帯3 { get; set; }
        //	修正調整CS帯4			CSV_COL名：	修正／調整：CS帯4	
        public string 修正調整CS帯4 { get; set; }
        //	修正調整CS帯5			CSV_COL名：	修正／調整：CS帯5	
        public string 修正調整CS帯5 { get; set; }
        //	検証改善CS帯1			CSV_COL名：	検証／改善：CS帯1	
        public string 検証改善CS帯1 { get; set; }
        //	検証改善CS帯2			CSV_COL名：	検証／改善：CS帯2	
        public string 検証改善CS帯2 { get; set; }
        //	検証改善CS帯3			CSV_COL名：	検証／改善：CS帯3	
        public string 検証改善CS帯3 { get; set; }
        //	検証改善CS帯4			CSV_COL名：	検証／改善：CS帯4	
        public string 検証改善CS帯4 { get; set; }
        //	検証改善CS帯5			CSV_COL名：	検証／改善：CS帯5	
        public string 検証改善CS帯5 { get; set; }
        //	前回受験者ID			CSV_COL名：	前回受験者ID	
        public string 前回受験者ID { get; set; }
        //	前回採点日			CSV_COL名：	[前回]採点日	
        public string 前回採点日 { get; set; }
        //	前回リテラシー総合レベル			CSV_COL名：	[前回]リテラシー総合：レベル	
        public string 前回リテラシー総合レベル { get; set; }
        //	前回リテラシー情報収集力レベル			CSV_COL名：	[前回]リテラシー情報収集力：レベル	
        public string 前回リテラシー情報収集力レベル { get; set; }
        //	前回リテラシー情報分析力レベル			CSV_COL名：	[前回]リテラシー情報分析力：レベル	
        public string 前回リテラシー情報分析力レベル { get; set; }
        //	前回リテラシー課題発見力レベル			CSV_COL名：	[前回]リテラシー課題発見力：レベル	
        public string 前回リテラシー課題発見力レベル { get; set; }
        //	前回リテラシー構想力レベル			CSV_COL名：	[前回]リテラシー構想力：レベル	
        public string 前回リテラシー構想力レベル { get; set; }
        //	前回コンピテンシー総合レベル			CSV_COL名：	[前回]コンピテンシー総合：レベル	
        public string 前回コンピテンシー総合レベル { get; set; }
        //	前回対人基礎力レベル			CSV_COL名：	[前回]対人基礎力：レベル	
        public string 前回対人基礎力レベル { get; set; }
        //	前回対自己基礎力レベル			CSV_COL名：	[前回]対自己基礎力：レベル	
        public string 前回対自己基礎力レベル { get; set; }
        //	前回対課題基礎力レベル			CSV_COL名：	[前回]対課題基礎力：レベル	
        public string 前回対課題基礎力レベル { get; set; }
        //	前回親和力レベル			CSV_COL名：	[前回]親和力：レベル	
        public string 前回親和力レベル { get; set; }
        //	前回協働力レベル			CSV_COL名：	[前回]協働力：レベル	
        public string 前回協働力レベル { get; set; }
        //	前回統率力レベル			CSV_COL名：	[前回]統率力：レベル	
        public string 前回統率力レベル { get; set; }
        //	前回感情制御力レベル			CSV_COL名：	[前回]感情制御力：レベル	
        public string 前回感情制御力レベル { get; set; }
        //	前回自信創出力レベル			CSV_COL名：	[前回]自信創出力：レベル	
        public string 前回自信創出力レベル { get; set; }
        //	前回行動持続力レベル			CSV_COL名：	[前回]行動持続力：レベル	
        public string 前回行動持続力レベル { get; set; }
        //	前回課題発見力レベル			CSV_COL名：	[前回]課題発見力：レベル	
        public string 前回課題発見力レベル { get; set; }
        //	前回計画立案力レベル			CSV_COL名：	[前回]計画立案力：レベル	
        public string 前回計画立案力レベル { get; set; }
        //	前回実践力レベル			CSV_COL名：	[前回]実践力：レベル	
        public string 前回実践力レベル { get; set; }
        //	前回親しみやすさレベル			CSV_COL名：	[前回]親しみやすさ：レベル	
        public string 前回親しみやすさレベル { get; set; }
        //	前回気配りレベル			CSV_COL名：	[前回]気配り：レベル	
        public string 前回気配りレベル { get; set; }
        //	前回対人興味共感受容レベル			CSV_COL名：	[前回]対人興味／共感・受容：レベル	
        public string 前回対人興味共感受容レベル { get; set; }
        //	前回多様性理解レベル			CSV_COL名：	[前回]多様性理解：レベル	
        public string 前回多様性理解レベル { get; set; }
        //	前回人脈形成レベル			CSV_COL名：	[前回]人脈形成：レベル	
        public string 前回人脈形成レベル { get; set; }
        //	前回信頼構築レベル			CSV_COL名：	[前回]信頼構築：レベル	
        public string 前回信頼構築レベル { get; set; }
        //	前回役割理解連携行動レベル			CSV_COL名：	[前回]役割理解・連携行動：レベル	
        public string 前回役割理解連携行動レベル { get; set; }
        //	前回情報共有レベル			CSV_COL名：	[前回]情報共有：レベル	
        public string 前回情報共有レベル { get; set; }
        //	前回相互支援レベル			CSV_COL名：	[前回]相互支援：レベル	
        public string 前回相互支援レベル { get; set; }
        //	前回相談指導他者の動機づけレベル			CSV_COL名：	[前回]相談・指導・他者の動機づけ：レベル	
        public string 前回相談指導他者の動機づけレベル { get; set; }
        //	前回話しあうレベル			CSV_COL名：	[前回]話しあう：レベル	
        public string 前回話しあうレベル { get; set; }
        //	前回意見を主張するレベル			CSV_COL名：	[前回]意見を主張する：レベル	
        public string 前回意見を主張するレベル { get; set; }
        //	前回建設的創造的な討議レベル			CSV_COL名：	[前回]建設的・創造的な討議：レベル	
        public string 前回建設的創造的な討議レベル { get; set; }
        //	前回意見の調整交渉説得レベル			CSV_COL名：	[前回]意見の調整、交渉、説得：レベル	
        public string 前回意見の調整交渉説得レベル { get; set; }
        //	前回セルフアウェアネスレベル			CSV_COL名：	[前回]セルフアウェアネス：レベル	
        public string 前回セルフアウェアネスレベル { get; set; }
        //	前回ストレスコーピングレベル			CSV_COL名：	[前回]ストレスコーピング：レベル	
        public string 前回ストレスコーピングレベル { get; set; }
        //	前回ストレスマネジメントレベル			CSV_COL名：	[前回]ストレスマネジメント：レベル	
        public string 前回ストレスマネジメントレベル { get; set; }
        //	前回独自性理解レベル			CSV_COL名：	[前回]独自性理解：レベル	
        public string 前回独自性理解レベル { get; set; }
        //	前回自己効力感楽観性レベル			CSV_COL名：	[前回]自己効力感／楽観性：レベル	
        public string 前回自己効力感楽観性レベル { get; set; }
        //	前回学習視点機会による自己変革レベル			CSV_COL名：	[前回]学習視点・機会による自己変革：レベル	
        public string 前回学習視点機会による自己変革レベル { get; set; }
        //	前回主体的行動レベル			CSV_COL名：	[前回]主体的行動：レベル	
        public string 前回主体的行動レベル { get; set; }
        //	前回完遂レベル			CSV_COL名：	[前回]完遂：レベル	
        public string 前回完遂レベル { get; set; }
        //	前回良い行動の習慣化レベル			CSV_COL名：	[前回]良い行動の習慣化：レベル	
        public string 前回良い行動の習慣化レベル { get; set; }
        //	前回情報収集レベル			CSV_COL名：	[前回]情報収集：レベル	
        public string 前回情報収集レベル { get; set; }
        //	前回本質理解レベル			CSV_COL名：	[前回]本質理解：レベル	
        public string 前回本質理解レベル { get; set; }
        //	前回原因追究レベル			CSV_COL名：	[前回]原因追究：レベル	
        public string 前回原因追究レベル { get; set; }
        //	前回目標設定レベル			CSV_COL名：	[前回]目標設定：レベル	
        public string 前回目標設定レベル { get; set; }
        //	前回シナリオ構築レベル			CSV_COL名：	[前回]シナリオ構築：レベル	
        public string 前回シナリオ構築レベル { get; set; }
        //	前回計画評価レベル			CSV_COL名：	[前回]計画評価：レベル	
        public string 前回計画評価レベル { get; set; }
        //	前回リスク分析レベル			CSV_COL名：	[前回]リスク分析：レベル	
        public string 前回リスク分析レベル { get; set; }
        //	前回実践行動レベル			CSV_COL名：	[前回]実践行動：レベル	
        public string 前回実践行動レベル { get; set; }
        //	前回修正調整レベル			CSV_COL名：	[前回]修正／調整：レベル	
        public string 前回修正調整レベル { get; set; }
        //	前回検証改善レベル			CSV_COL名：	[前回]検証／改善：レベル	
        public string 前回検証改善レベル { get; set; }

    }

}
