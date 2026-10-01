import pyodbc

conn = pyodbc.connect(
    'DRIVER={ODBC Driver 17 for SQL Server};'
    'SERVER=COMP11A1\\SQLEXPRESS;'
    'DATABASE=Primer;'
    'Trusted_Connection=yes;'
    'TrustServerCertificate=yes;')

cursor = conn.cursor()


def get_name(number):
    table_name = ["customers", "sales", "customers_product"]
    return table_name[number]

def get_date(select: int):
    query = [f"CREATE TABLE {get_name()}(id int IDENTITY PRIMARY KEY, quantity int NOT NULL, created_at datetime DEFAULT GetDate())",
        "INSERT INTO orders(quantity) VALUES(?)",
        "SELECT TOP 50 id, quantity, created_at FROM orders"]
    return query[select]


def create_table(table_name: str):
    cursor.execute(get_date(0))
    cursor.commit()

def save_order(quatity: int):
    cursor.execute(get_date(1), quatity)
    cursor.commit()

def show_collection():
    cursor.execute(get_date(2))
    row = cursor.fetchall()
    if row:
        print(row)

def main():
    for i in range(2):
        table  = get_name(i)
        create_table(table)
        save_order(i + 1, table)
        show_collection(table)


if __name__ == "__main__":
    main()