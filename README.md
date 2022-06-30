# FSDresser: ボーンを入れ子にするやつ

[FLASTOREアバター](https://flastore.booth.pm/) に服を着せるツールです。

通常は [AvatarTools](https://booth.pm/ja/items/1564788) やその派生ツールで大部分の着せ替えニーズをカバーできるはずですが、素体と服でバージョンが違うなどした場合、[手動での合成手順](https://drive.google.com/file/d/1eXksqtu9eiGK6ERvU5PSjwCXbrY4Uia9/view?usp=sharing) を行うことがあります。このツールは上記手順書において `c` ~ `e` を自動化します。

## 前提条件

- 上記手順書で導入できる素体と服であること
- Unity 2019.4.31f1
- DynamicBone, PhysBone, Contacts, Constraint などのコンポーネントの参照は更新しません。AvatarTools のようなボーンの合成は行わないため必要ないはずですが、もし必要な時はそこは手作業となります。

## インストール

*.unitypackage としてダウンロードしてきたときは、いつものように Unity エディタにインポートしてください。

GitHub から zip をダウンロードしてきたときは、Unity プロジェクトの Assets/ 以下にフォルダ構造を維持して展開してください。

## つかいかた

Unity エディタの Window/Autch/FSDresser を選んでウィンドウを開きます。

ウィンドウの操作はほぼ AvatarTools に合わせてあります。

ウィンドウの「着せるアバター」に服を着せたいアバターの GameObject をドラッグします。使えるオブジェクトの条件は以下の通りです。

- 直下に Armature があり、その下に Hips がある
- ドラッグした GameObject に Animator コンポーネントが設定されている

「アイテムリストを増減」の + - ボタンで「着せたいアイテムのリスト」の個数を増減します。出てきた「着せるアイテム (n)」にそれぞれ着せたい服をドラッグします。

準備ができたら「合成」ボタンを押します。成功すれば「成功！」とダイアログが出て、Hierarchy に合成後のアバターが増えて選択状態になります。アバターの合成処理は複製を作ってそこで行われるため、ドラッグしたオブジェクトは変更されません。

合成に失敗するとその旨のダイアログが出ます。「コンソール」を開くとエラーの内容と原因となったオブジェクトが参照できます。


## ライセンス

Copyright (c) 2022, Autch.net

MIT ライセンスです。

