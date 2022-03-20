sleep 20

curl -X POST 'http://nacos:8848/nacos/v1/cs/configs' -d $'dataId=dicloud&group=DEFAULT_GROUP&type=yaml&content=dataSource:%0A%20%20DiSystem:%0A%20%20%20%20type: MySQL%0A%20%20%20%20configuration: server=mysql.storage.svc.clus'

dotnet WebApplication1.dll