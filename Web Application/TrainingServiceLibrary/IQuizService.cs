using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQuizService" in both code and config file together.
    [ServiceContract]
    public interface IQuizService
    {
        [OperationContract]
        bool AddQuiz(QuizTransfer quiz);

        [OperationContract]
        bool UpdateQuiz(QuizTransfer quiz);

        [OperationContract]
        List<QuizTransfer> GetQuiz(string key = null);

        [OperationContract]
        bool Delete(List<Int32> quizIdList);

        [OperationContract]
        bool AddToModule(int quizId, int moduleId);

        [OperationContract]
        bool SaveAccountAnswer(List<QuizAccountAnswerAccess> quizAccountAnswers);

        [OperationContract]
        bool SaveQuizRecord(QuizRecordAccess quizRecord);

        [OperationContract]
        List<QuizTransfer> GetQuizForModule(int moduleId);
    }
}
