using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Transactions;

using Acr.Business.Concrete;
using Acr.Core.Models.Request;
using Acr.DataAccess.Concrete;
using Acr.Entities.Concrete;
using Acr.Entities.Views;


namespace Acr.UnitTest
{
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

                    var getRequest = new GetRequest { Id = entity.Id };
                    var getResult = testManager.GetFromViewById<ViewTestTable>(getRequest);
                    Assert.IsTrue(getResult.Success);
                    Debug.WriteLine(getResult.Success + ", " + getResult.Message);

                    var deleteRequest = new GetRequest { Id = entity.Id };
                    var deleteResult = testManager.Delete(deleteRequest);
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

                    var getRequest = new GetRequest { Id = entity.Id };
                    var getResult = testJsonManager.GetWithObject<TestJson>(getRequest);
                    Assert.IsTrue(getResult.Success);

                    entity.Name = "xxxx";
                    entity.ExtraObject.Num = 2;
                    var updateResult = testJsonManager.UpdateWithObject<TestJson>(entity);
                    Debug.WriteLine(updateResult.Success + ", " + updateResult.Message);
                    Assert.AreEqual(updateResult.Success, true);

                    var getRequest2 = new GetRequest { Id = entity.Id };
                    var getResult2 = testJsonManager.GetWithObject<TestJson>(getRequest2);
                    Assert.IsTrue(getResult2.Success);
                    Debug.WriteLine(getResult2.Success + ", " + getResult2.Message);

                    var deleteRequest = new GetRequest { Id = entity.Id };
                    var deleteResult = testJsonManager.Delete(deleteRequest);
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