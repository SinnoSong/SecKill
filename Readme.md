# 九价疫苗秒杀程序Windows版
九价秒杀工具WPF版，代码主要业务逻辑参考java项目：https://github.com/lyrric/seckill 。  
本软件**仅支持秒苗小程序**，不支持其他小程序。  
## 运行环境
**基于.net frameword4.7.2开发，win10 1803版之后版本直接可以运行，之前版本的win10、win7、win8.1需要安装.net framework 4.7.2运行**，下载链接：https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net472
## 使用
1. 打开exe文件，选择好要秒杀区域的省市，点击确定。  
2. 先打开**fiddler（抓包工具，使用请自行搜索）**，再打开微信PC版打开秒苗小程序。  
3. 在秒苗小程序上点击**我的**=>**家庭成员管理**。Fiddler中可以看到一个链接是[miaomiao.scmttec.com/seckill/linkman/findByUserId.do](miaomiao.scmttec.com/seckill/linkman/findByUserId.do)请求被抓取到。
4. Fiddler软件中右侧点击**Inspectors**，再点击**Raw**，在下面显示内容中复制**tk,Cookie**的值。
5. 点击**设置Cookie按钮**将上一步抓取的**tk,Cookie**值复制到**tk,Cookie**文本框中，点击**保存**。
6. 点击**选择成员**，选择要秒杀的成员，点击**确定**。
7. 点击**刷新疫苗列表**，在显示的疫苗列表中选择要秒杀的疫苗，设置好提前多少毫秒开始抢苗和每次抢苗间隔后，点击**开始秒杀**。
**软件到秒杀时间后会自动运行**
## 使用注意
1. **本软件和秒苗小程序不能同时运行！**
2. 抓包获取的token最好是**30分钟**内，如果在软件获取用户或获取疫苗列表时提示**token过期**，请重新使用**fiddler**软件进行抓包，获取最新的token和cookie。
3. 使用前请确认秒苗小程序目前有可预约疫苗。如果秒苗小程序上没有显示你所使用的地区有可使用的疫苗，本软件也不会任何疫苗信息。
4. 如果抓包异常，无法抓取到任何信息，请参考该链接：[fiddler抓包解决](https://www.jianshu.com/p/f87512ed7b21)。  
    具体操作：  
    进入*C:\Users\你的用户名称\AppData\Roaming\Tencent\WeChat\XPlugin\Plugins\WMPFRuntime*将里面的文件夹全部删除。  
    **注意：删除时需要确保微信已退出**
5. 如果要更改秒杀开始时间，秒杀间隔。请直接在界面上进行设置单位毫秒，不填写会进行默认配置


## 抓包信息示例：
**cookie:**   _xxhm_=%7B%22id%22%3A19929155%2C%22mobile%22%3A%2218790930867%22%2C%22nickName%22%3A%22%E5%B0%8F%E7%99%BD%E3%80%82%22%2C%22headerImg%22%3A%22http%3A%2F%2Fthirdwx.qlogo.cn%2Fmmopen%2Fic9BcyRDyOItyFglnKcpwLwMTQpl8ZyIxU52w0DNRt0VjqlVaSZpweFDngMLBFJGNLmzsZlHOK7GwS79cgM9iaibYgzu0csFtH2%2F132%22%2C%22regionCode%22%3A%22330106%22%2C%22name%22%3A%22%E5%AE%8B**%22%2C%22uFrom%22%3A%22depa_vacc_detail%22%2C%22wxSubscribed%22%3A1%2C%22birthday%22%3A%221998-03-05+02%3A00%3A00%22%2C%22sex%22%3A1%2C%22hasPassword%22%3Afalse%2C%22birthdayStr%22%3A%221998-03-05%22%7D;   
**token:**   
wxapptoken:10:848b46edd7fe69b98680bd3541f29d56_bf246bba2d811ddc1c335f7a98305b51

## 感谢
<img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.png"  width="100px">
