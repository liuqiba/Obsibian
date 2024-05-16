#python 
### 常用命令 
1. 导出依赖库到requirements.tex  #requirements
	   -   运行以下命令：pip install pipreqs
	-   等待安装完成后，运行以下命令：pipreqs ./ --encoding=utf-8 --force
	-    pip install -r requirements.txt
	-    更换源 pip install -r requirements.txt -i https://pypi.tuna.tsinghua.edu.cn/simple
			
 2. 简单文件服务器 #文件服务
    ```python
python3 -m http.server 《port》
```
    