import mssql_python
if __name__ == '__main__':
    connection_string = (
        "SERVER=COMP11A1\\SQLEXPRESS;"
        "DATABASE=Primer;"
        "Trusted_Connection=yes;"
        "Encrypt=yes;"
        "TrustServerCertificate=yes;"
    )
    connection = mssql_python.connect(connection_string)

    cursor = connection.cursor()

    cursor.execute("""
        SELECT TOP 10 *
        FROM dbo.Addresses    
    """)
    rows = cursor.fetchall()

    for row in rows:
        print(row)

    connection.close()


