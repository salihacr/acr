using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Transactions;

using Acr.Business.Concrete;
using Acr.Core.Models.Request;
using Acr.DataAccess.Concrete;
using Acr.Entities.Concrete;
using Acr.Entities.Views;
using System.Linq.Expressions;

namespace Acr.UnitTest
{

    public class RepositoryTest
    {
        [TestMethod]
        public void GetList_Test()
        {
            try
            {
                var repository = new BaseRepository<TestTable>(new DataAccess.AppDbContext());

                //repository.GetList(
                //    pageNo: 1,
                //    pageSize: 50,
                //    predicate: (x => x.Name.Contains("x")),
                //    orderBy: (x=>x.)),
                //    include:null

                //    );


            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(false);
                Debug.WriteLine(ex.ToString());
            }
        }
    }

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTableCrudTest()
        {
            try
            {
                var testManager = new BaseManager<TestTable>(
                new BaseRepository<TestTable>(new DataAccess.AppDbContext()),
                new LogRepository(new DataAccess.AppDbContext()));


                var entity = new TestTable
                {
                    Name = "xxx",
                };

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    var createResult = testManager.Add(entity);
                    Debug.WriteLine(createResult.Success + " " + createResult.Message);
                    Assert.AreEqual(createResult.Success, true);

                    entity.Name = "xxxx";
                    var updateResult = testManager.Update(entity);
                    Debug.WriteLine(updateResult.Success + ", " + updateResult.Message);
                    Assert.AreEqual(updateResult.Success, true);

                    var getResult = testManager.GetFromViewById<ViewTestTable>(entity.Id);
                    Assert.IsTrue(getResult.Success);
                    Debug.WriteLine(getResult.Success + ", " + getResult.Message);

                    var deleteResult = testManager.Delete(entity.Id);
                    Debug.WriteLine(deleteResult.Success + " " + deleteResult.Message);
                    Assert.AreEqual(deleteResult.Success, true);

                    transactionScope.Complete();
                }
                var result = testManager.GetAllFromView<ViewTestTable>(new PaginationRequest { PageNo = 1, PageSize = 50 });
                Assert.IsTrue(result.Success);
                Debug.WriteLine(result.Success + ", " + result.Message);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Assert.AreEqual(true, false);
            }
        }



        [TestMethod]
        public void TestJsonTableCrudTest()
        {
            try
            {
                var testJsonManager = new BaseManager<TestJsonTable>(
                new BaseRepository<TestJsonTable>(new DataAccess.AppDbContext()),
                new LogRepository(new DataAccess.AppDbContext()));


                var entity = new TestJsonTable
                {
                    Name = "test-name",
                    ExtraObject = new TestJson
                    {
                        Num = 1,
                        Str = "str-val",
                    },
                };

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    var createResult = testJsonManager.AddWithObject<TestJson>(entity);
                    Debug.WriteLine(createResult.Success + " " + createResult.Message);
                    Assert.AreEqual(createResult.Success, true);

                    var getResult = testJsonManager.GetWithObject<TestJson>(entity.Id);
                    Assert.IsTrue(getResult.Success);

                    entity.Name = "xxxx";
                    entity.ExtraObject.Num = 2;
                    var updateResult = testJsonManager.UpdateWithObject<TestJson>(entity);
                    Debug.WriteLine(updateResult.Success + ", " + updateResult.Message);
                    Assert.AreEqual(updateResult.Success, true);

                    var getResult2 = testJsonManager.GetWithObject<TestJson>(entity.Id);
                    Assert.IsTrue(getResult2.Success);
                    Debug.WriteLine(getResult2.Success + ", " + getResult2.Message);

                    var deleteResult = testJsonManager.Delete(entity.Id);
                    Debug.WriteLine(deleteResult.Success + " " + deleteResult.Message);
                    Assert.AreEqual(deleteResult.Success, true);

                    transactionScope.Complete();
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Assert.AreEqual(true, false);
            }
        }

    }
}