# uwp-messageCenter

这是一个类似iOS里的NotificationCenter库。
提供应用内不同对象之间的互动（尤其是没有直接关联的对象）。

文件结构：
```javascript
Noear.UWP.Data{
    MessageCenter
}
```

示例代码：
```java
//Page1.cs ----------
//
protected override void OnNavigatedTo(NavigationEventArgs e) {
  //进入页面时订阅消息"On_Addin_Add"
  MessageCenter.Subscribe("On_Addin_Add", this, (args) => { 
      if (viewModel == null)
          return;

      if (args.Length > 0) {
          viewModel.tileList.Add(args[0] as HubTileModel);
      }
  });
}
protected override void OnNavigatedFrom(NavigationEventArgs e){
  if (e.NavigationMode == NavigationMode.Back) {
    //退出时，取消对"On_Addin_Add"的订阅
    MessageCenter.UnSubscribe("On_Addin_Add", this); 
  }
}

//Page2.cs ----------
//
void AddinItem_Click(){
  //给消息中心发一个消息，并传过去一个参数
  MessageCenter.SendMessage("On_Addin_Add", new HubTileModel(m));
}

```
