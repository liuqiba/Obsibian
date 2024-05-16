**简介**：图形处理器（英语：graphics processing unit，缩写：GPU），又称显示核心、视觉处理器、显示芯片，是一种专门在个人电脑、工作站、游戏机和一些移动设备（如平板电脑、智能手机等）上做图像和图形相关运算工作的微处理器。GPU [算力](https://so.csdn.net/so/search?q=%E7%AE%97%E5%8A%9B&spm=1001.2101.3001.7020)的快速迭代升级，GPU 算力资源已经成为 AI 计算不可或缺的基础设施，可以说在这一轮 AI 发展浪潮中，AI 和 GPU 是相互成就。GPU 算力的不断提升，带动 AI 计算突破了算力瓶颈，使 AI 得以大规模的应用；AI 大规模应用以及越来越大规模的模型，也反过来带动了 GPU 算力的不断提升。

**查看 Windows 的 CUDA 版本及 GPU 信息：**

```
nvidia-smi 
```

**执行结果：**

![](https://img-blog.csdnimg.cn/515aee86e5f2487e9858abcea707cbc2.png)

**Python 中可通过 nvidia-ml-py 模块进行查询**

**安装：**

```
pip install nvidia-ml-py
```

**使用：**

```
# -*- coding: utf-8 -*-
# time: 2022/8/18 08:22
# file: nv_test.py
# 公众号: 玩转测试开发
from pynvml import *

def get_nv_smi():
    handle = nvmlDeviceGetHandleByIndex(0)  # 0 是 GPU id
    meminfo = nvmlDeviceGetMemoryInfo(handle)
    print(meminfo.total / 1024 / 1024)  # 显存大小
    print(meminfo.used / 1024 / 1024)  # 单位：字节bytes，所以要想得到以兆M为单位就需要除以1024**2
    print(meminfo.free / 1024 / 1024)  # 显卡剩余显存大小
    print(nvmlDeviceGetCount())  # 查询显示GPU数目

def nvidia_info():
    nvidia_dict = {
        "state": True,
        "nvidia_version": "",
        "nvidia_count": 0,
        "gpus": []
    }
    try:
        nvmlInit()
        nvidia_dict["nvidia_version"] = nvmlSystemGetDriverVersion()
        nvidia_dict["nvidia_count"] = nvmlDeviceGetCount()
        for i in range(nvidia_dict["nvidia_count"]):
            handle = nvmlDeviceGetHandleByIndex(i)
            memory_info = nvmlDeviceGetMemoryInfo(handle)
            gpu = {
                "gpu_name": nvmlDeviceGetName(handle),
                "total": memory_info.total,
                "free": memory_info.free,
                "used": memory_info.used,
                "temperature": f"{nvmlDeviceGetTemperature(handle, 0)}℃",
                "powerStatus": nvmlDeviceGetPowerState(handle)
            }
            nvidia_dict['gpus'].append(gpu)
    except NVMLError as _:
        nvidia_dict["state"] = False
    except Exception as _:
        nvidia_dict["state"] = False
    finally:
        try:
            nvmlShutdown()
        except:
            pass
    return nvidia_dict

def check_gpu_mem_usedRate():
    max_rate = 0.0

    while True:
        info = nvidia_info()
        print(info)
        used = info['gpus'][0]['used']
        total = info['gpus'][0]['total']
        temperature = info['gpus'][0]['temperature']
        print(f"GPU0 used: {used}, total: {total}, 使用率：{used/total}")
        if used / total > max_rate:
            max_rate = used / total
        print("GPU0 最大使用率：", max_rate)

        if int(temperature.split("℃")[0]) >= 52:
            break

if __name__ == '__main__':
    nvmlInit()  # 需要先初始化。

    get_nv_smi()
    nvidia_info()
    check_gpu_mem_usedRate()
```

**执行结果：**

```
2048.0
64.2578125
1983.7421875

{'state': True, 'nvidia_version': '516.01', 'nvidia_count': 1, 'gpus': [{'gpu_name': 'NVIDIA GeForce MX150', 'total': 2147483648, 'free': 2080104448, 'used': 67379200, 'temperature': '50℃', 'powerStatus': 0}]}
GPU0 used: 67379200, total: 2147483648, 使用率：0.031375885009765625
GPU0 最大使用率：0.031375885009765625
```

![](https://img-blog.csdnimg.cn/d44b6dae440f489ca92ba1c856a10d31.png)

**微信公众号：玩转测试开发  
欢迎关注，共同进步，谢谢！**