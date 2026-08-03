# チーム名

IoAteamMR

# プロダクト名

MR原始人フィットネス

## 概要

運動不足解消のためのフィットネスMRゲーム
世界観は原始人になって様々な運動をするというもの
部位ごとにトレーニングできるようにモードを分けている
腕⇒チャンバラ　肩⇒槍投げ　脚⇒縄跳び　

## デモ

<img width="3840" height="2160" alt="com UnityTechnologies com unity template urpblank-20260803-093814" src="https://github.com/user-attachments/assets/545ac472-d745-485b-8df3-6f988a71f360" />
（チャンバラゲーム）

<img width="3840" height="2160" alt="com UnityTechnologies com unity template urpblank-20260803-094055" src="https://github.com/user-attachments/assets/10ccc16d-98e8-405a-a602-1120904e9586" />
（縄跳びゲーム）

<img width="3840" height="2160" alt="com UnityTechnologies com unity template urpblank-20260803-094627" src="https://github.com/user-attachments/assets/588c960a-fa74-4e24-ae29-4b85e93310ca" />
（槍投げゲーム）

## システム構成

<img width="1280" height="720" alt="システム設計" src="https://github.com/user-attachments/assets/116e5b97-40b8-4b8d-b802-9132eaf8bc5c" />
メインメニューからすべてのゲームにつながっており選択した部位によって映るシーンが変わる


## 背景・課題

なぜこのプロダクトを作ったのか、どのような課題を解決したいのかを記載してください。
近年の発展に伴い、人々は動かずとも作業でき生活できる環境を手に入れることが出来ています。しかし、その反面として運動不足になり、体力の低下や生活習慣病にかかりやすくなるといった問題が顕著にみられるようになりました。
そこで、私たちは人の生活の発展を原初からやり直し、原始人のやっていた運動を再現することで運動不足の解消を図ろうと考えました！！
今回のHackitのテーマである「突破」と掛け合わせ、体力の「限界”突破”」を目標にMRを用いてフィットネスゲームを作成し、運動不足の解消を図ります！！


## 主な機能
機能1　「ゲーム１：棍棒チャンバラ」棍棒を降って敵原始人に指定した回数攻撃することで、敵を撃破して腕を鍛えよう！
機能2　「ゲーム２：ツタ跳び」指定回数ツタを縄跳びして脚を鍛えよう！
機能3　「ゲーム３：槍投げ」槍を投げてマンモスを倒しながら肩を鍛えよう！
工夫した点・こだわった点
metaクエストのパススルー機能を使って現実世界で原始時代を楽しめるように工夫できました。また、Unityの役割分担で一人がゲームシステム開発、もう一人でMRへの導入というように役割しましたが、お互いの強みを活かしたいい役割分担となり、Githubも使いこなすことができました。しかし、Aseetの違いなどでエラーも多く苦労しました

## 使用技術

フロントエンド：Unity6000.3.12f1 (C#)、Unity UI (uGUI)、XR Interaction Toolkit

バックエンド：なし（ローカル処理のみ）

AI / API：Meta XR SDK、Scene Understanding API、OpenXR、VPS（Visual Positioning Service）

データベース：なし（ローカル処理のみ）

インフラ：Meta Quest 3、Git・GitHub（ソース管理）

その他：Blender（3Dモデル作成）、Shader Graph（マテリアル作成）、GitHub Desktop（バージョン管理）

## 今後の展望
「ゲーム４：薪割り」の追加や、各ゲームのHPやエフェクトの実装をしてより面白味があり
やりこめるゲームにしたいです！

## セットアップ方法

Meta Quest3を準備して接続できるようにする


## メンバー

|　 名前 　|  担当  |
| 小澤洋太 |unity‣MR|
| 喜多希海 | unity |
| 石田結誠 |blender|
| 茂木陽生 |blender|
