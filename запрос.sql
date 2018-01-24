SELECT     distinct c.Name
FROM       customers c 
inner JOIN product p
ON         (c.customerid = p.customerid)
WHERE      c.customerid IN
           (	SELECT p1.customerid
                  FROM   product p1
                  WHERE  p1.productname='Молоко')
AND        c.customerid NOT IN
           (	SELECT p2.customerid
                  FROM   product p2
                  WHERE  p2.productname='Сметана'
                  AND    p2.purchaisedatetime> Now() - interval 1 month)  