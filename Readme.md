九价秒杀工具WPF版，代码主要业务逻辑参考java项目：https://github.com/lyrric/seckill 。

本软件仅支持秒苗小程序，具体使用链接参考文章参考：https://zhuanlan.zhihu.com/p/449082615

## 运行环境
**基于.net frameword4.7.2开发，win10 1803版之后版本直接可以运行，之前版本的win10、win7、win8.1需要安装.net framework 4.7.2运行**，下载链接：https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net472

## 使用注意
1. 本软件和秒苗小程序不能同时运行！
2. 使用前请确认秒苗小程序目前有可预约疫苗。如果秒苗小程序上没有显示你所使用的地区有可使用的疫苗，本软件也不会任何疫苗信息。
3. 如果抓包异常，无法抓取到任何信息，请更换一台电脑。
4. 秒杀开始时段为开始秒杀前50毫秒，发出第一个请求后，每个100毫秒会再次发送共发送4次。
    注：如果要更改秒杀开始时间，秒杀间隔。请求修改/Service/SecKillService.cs文件中下图位置
    ![changeSecKillImg](https://user-images.githubusercontent.com/59649274/163344594-968879b5-070d-4bd6-b3de-33706ac4eaf8.png)


#### 需要抓包信息示例：
**cookie:** _xxhm_=%7B%22id%22%3A19929155%2C%22mobile%22%3A%2218790930867%22%2C%22nickName%22%3A%22%E5%B0%8F%E7%99%BD%E3%80%82%22%2C%22headerImg%22%3A%22http%3A%2F%2Fthirdwx.qlogo.cn%2Fmmopen%2Fic9BcyRDyOItyFglnKcpwLwMTQpl8ZyIxU52w0DNRt0VjqlVaSZpweFDngMLBFJGNLmzsZlHOK7GwS79cgM9iaibYgzu0csFtH2%2F132%22%2C%22regionCode%22%3A%22330106%22%2C%22name%22%3A%22%E5%AE%8B**%22%2C%22uFrom%22%3A%22depa_vacc_detail%22%2C%22wxSubscribed%22%3A1%2C%22birthday%22%3A%221998-03-05+02%3A00%3A00%22%2C%22sex%22%3A1%2C%22hasPassword%22%3Afalse%2C%22birthdayStr%22%3A%221998-03-05%22%7D; 

**token:**
wxapptoken:10:848b46edd7fe69b98680bd3541f29d56_bf246bba2d811ddc1c335f7a98305b51
