using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QuizService" in both code and config file together.
    public class QuizService : IQuizService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        public bool AddQuiz(QuizTransfer quiz)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            Int32 quizIdTemp = 0;

            //insert a new record to Quiz table
            cmd = new SqlCommand("AddQuiz");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter quizId = cmd.Parameters.Add("@quizId", SqlDbType.Int);
            quizId.Direction = ParameterDirection.Output;

            SqlParameter moduleId = cmd.Parameters.Add("@moduleId", SqlDbType.Int);
            moduleId.Direction = ParameterDirection.Input;

            SqlParameter passingGrade = cmd.Parameters.Add("@passingGrade", SqlDbType.Int);
            passingGrade.Direction = ParameterDirection.Input;

            SqlParameter quizName = cmd.Parameters.Add("@quizName", SqlDbType.VarChar, 512);
            quizName.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 512);
            description.Direction = ParameterDirection.Input;

            if (quiz.QuizDescription.QuizName != null)
            {
                quizName.Value = quiz.QuizDescription.QuizName;
            }
            if (quiz.QuizDescription.Description != null)
            {
                description.Value = quiz.QuizDescription.Description;
            }

            passingGrade.Value = quiz.QuizDescription.PassingGrade;

            if (quiz.QuizDescription.ModuleId != default(Int32))
            {
                moduleId.Value = quiz.QuizDescription.ModuleId;
            }
            
            db.changeData(cmd);

            quizIdTemp = (int)quizId.Value;

            //insert new records into QuestionAnswer table
            if ((int)cmd.Parameters["RetVal"].Value == 0)
            {
                //insert bool question data into database
                for (int i = 0; i < quiz.BoolQuestionNumber; i++)
                {
                    bool blReturn = false;
                    int questionId;
                    blReturn = addQuestionTable(db, cmd, quiz.BoolQuestion.ElementAt(i), quizIdTemp, out questionId);
                    if (blReturn == false)
                    {
                        db.Close();
                        return false;
                    }
                }

                //insert multiselection question data into database
                for (int i = 0; i < quiz.MulSelQuestionNumber; i++)
                {
                    bool blReturn = false;
                    int questionId;
                    blReturn = addQuestionTable(db, cmd, quiz.MulSelQuestion.ElementAt(i).QuestionAnswer, quizIdTemp, out questionId);
                    if (blReturn == false)
                    {
                        db.Close();
                        return false;
                    }

                    //insert selection items data into database
                    int itemNumber = quiz.MulSelQuestion.ElementAt(i).ItemsNumber;
                    for (int j = 0; j < itemNumber; j++)
                    {
                        string questionDescription = quiz.MulSelQuestion.ElementAt(i).QuestionAnswer.QuestionDescription;
                        string type = quiz.MulSelQuestion.ElementAt(i).QuestionAnswer.Type;
                        string answer = quiz.MulSelQuestion.ElementAt(i).QuestionAnswer.Answer;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO Qustion_Answer_Multiple_Selection(questionAnswerId,choiceDescription,choiceSequenceNumber) VALUES "
                            +"(@questionId"+ i + j
                            + ", @choiceDescription" + i + j
                            + ", @choiceSequenceNumber" + i + j
                            +");";
                        cmd.Parameters.AddWithValue("@questionId" + i + j, questionId);
                        cmd.Parameters.AddWithValue("@choiceDescription" + i + j, quiz.MulSelQuestion.ElementAt(i).SelectionItems.ElementAt(j).ChoiceDescription);
                        cmd.Parameters.AddWithValue("@choiceSequenceNumber" + i + j, quiz.MulSelQuestion.ElementAt(i).SelectionItems.ElementAt(j).ChoiceSequenceNumber);
                        db.changeData(cmd);
                    }
                }
            }
            else
            {
                db.Close();
                return false;
            }

            db.Close();
            return true;  
        }

        /// <summary>
        /// insert data into QuestionAnswer table
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cmd"></param>
        /// <param name="question"></param>
        /// <param name="quizIdTemp"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public bool addQuestionTable(ConnectDB db, SqlCommand cmd, QuestionAnswerAccess question, int quizIdTemp, out int questionId)
        {
            cmd = new SqlCommand("AddQuestion");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetValQ1 = cmd.Parameters.Add("RetValQ1", SqlDbType.Int);
            RetValQ1.Direction = ParameterDirection.ReturnValue;

            SqlParameter questionAnswerId = cmd.Parameters.Add("@questionAnswerId", SqlDbType.Int);
            questionAnswerId.Direction = ParameterDirection.Output;

            SqlParameter quizIdQ = cmd.Parameters.Add("@quizId", SqlDbType.Int);
            quizIdQ.Direction = ParameterDirection.Input;

            SqlParameter questionDescription = cmd.Parameters.Add("@questionDescription", SqlDbType.VarChar, 255);
            questionDescription.Direction = ParameterDirection.Input;

            SqlParameter type = cmd.Parameters.Add("@type", SqlDbType.VarChar, 35);
            type.Direction = ParameterDirection.Input;

            SqlParameter Answer = cmd.Parameters.Add("@Answer", SqlDbType.VarChar, 255);
            Answer.Direction = ParameterDirection.Input;

            quizIdQ.Value = quizIdTemp;

            if (question.QuestionDescription != null)
            {
                questionDescription.Value = question.QuestionDescription;
            }
            if (question.Type != null)
            {
                type.Value = question.Type;
            }

            if (question.Answer != default(string))
            {
                Answer.Value = question.Answer;
            }

            db.changeData(cmd);

            questionId = (int)cmd.Parameters["@questionAnswerId"].Value;

            if ((int)cmd.Parameters["RetValQ1"].Value != 0)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<QuizTransfer> GetQuiz(string key = null)
        {
            List<QuizTransfer> quizList = new List<QuizTransfer>();
            
            string currentQuestionType;
            int currentQuizId = 0;
            int currentQuestionId = 0;

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (key == null)
            {
                cmd.CommandText = "SELECT DISTINCT * FROM Quiz AS Q LEFT JOIN Question_Answer AS QA ON Q.quizId = QA.quizId"
                                + " LEFT JOIN Qustion_Answer_Multiple_Selection AS QAMS ON QA.questionAnswerId = QAMS.questionAnswerId;";
            }
            else
            {
                cmd.CommandText = "SELECT DISTINCT * FROM Quiz AS Q LEFT JOIN Question_Answer AS QA ON Q.quizId = QA.quizId"
                                + " LEFT JOIN Qustion_Answer_Multiple_Selection AS QAMS ON QA.questionAnswerId = QAMS.questionAnswerId"
                                + " WHERE Q.quizName LIKE @quizName;";

                cmd.Parameters.Add("@quizName", SqlDbType.VarChar).Value = "%" + key + "%";
            }

            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //add data to quiz table
                    //quizId, quizName, description, passingGrade
                    QuizTransfer quizTransfer = new QuizTransfer();
                    
                    if (currentQuizId != Convert.ToInt32(dr[0])) 
                    {
                        QuizAccess quiz = new QuizAccess();

                        if (System.DBNull.Value.Equals(dr[0]))
                        {
                            quiz.QuizId = 0;  
                        }
                        else
                        {
                            quiz.QuizId = Convert.ToInt32(dr[0]);
                        }

                        currentQuizId = quiz.QuizId;

                        if (System.DBNull.Value.Equals(dr[1]))
                        {
                            quiz.QuizName = null;
                        }
                        else
                        {
                            quiz.QuizName = Convert.ToString(dr[1]);
                        }

                        if (System.DBNull.Value.Equals(dr[2]))
                        {
                            quiz.Description = null;
                        }
                        else
                        {
                            quiz.Description = Convert.ToString(dr[2]);
                        }

                        if (System.DBNull.Value.Equals(dr[3]))
                        {
                            quiz.PassingGrade = 0;
                        }
                        else
                        {
                            quiz.PassingGrade = Convert.ToInt32(dr[3]);
                        }

                        if (System.DBNull.Value.Equals(dr[4]))
                        {
                            quiz.ModuleId = 0;
                        }
                        else
                        {
                            quiz.ModuleId = Convert.ToInt32(dr[4]);
                        }

                        quizTransfer.QuizDescription = quiz;
                        //add quiz table to quizTransfer
                        if(currentQuizId != 0)
                        {
                            quizList.Add(quizTransfer);
                        }   
                    }

                    //add data to Question_Answer table
                    //questionAnswerId, quizId, questionDescription, type, Answer
                    if (!System.DBNull.Value.Equals(dr[5]))
                    {
                        if (currentQuestionId != Convert.ToInt32(dr[5]))
                        {
                            QuestionAnswerAccess question = new QuestionAnswerAccess();
                            QuestionTransfer mulQuestion = new QuestionTransfer();

                            if (System.DBNull.Value.Equals(dr[5]))
                            {
                                question.QuestionAnswerId = 0;
                            }
                            else
                            {
                                question.QuestionAnswerId = Convert.ToInt32(dr[5]);
                            }

                            currentQuestionId = question.QuestionAnswerId;

                            if (System.DBNull.Value.Equals(dr[6]))
                            {
                                question.QuizId = 0;
                            }
                            else
                            {
                                question.QuizId = Convert.ToInt32(dr[6]);
                            }

                            if (System.DBNull.Value.Equals(dr[7]))
                            {
                                question.QuestionDescription = null;
                            }
                            else
                            {
                                question.QuestionDescription = Convert.ToString(dr[7]);
                            }

                            if (System.DBNull.Value.Equals(dr[8]))
                            {
                                question.Type = null;
                            }
                            else
                            {
                                question.Type = Convert.ToString(dr[8]);
                            }

                            currentQuestionType = question.Type;

                            if (System.DBNull.Value.Equals(dr[9]))
                            {
                                question.Answer = null;
                            }
                            else
                            {
                                question.Answer = Convert.ToString(dr[9]);
                            }

                            if (currentQuestionType == "bool")
                            {
                                quizList.Last().BoolQuestion.Add(question);
                                quizList.Last().BoolQuestionNumber++;
                            }
                            else
                            {
                                mulQuestion.QuestionAnswer = question;
                                quizList.Last().MulSelQuestion.Add(mulQuestion);
                                quizList.Last().MulSelQuestionNumber++;
                            }
                        }
                    }
                    

                    //add data to Qustion_Answer_Multiple_Selection table
                    //mulSelectionId, questionAnswerId, choiceDescription, choiceSequenceNumber

                    if (!System.DBNull.Value.Equals(dr[10]))
                    {
                        QuestionSelectItem selectItem = new QuestionSelectItem();
                        selectItem.MulSelectionId = Convert.ToInt32(dr[10]);

                        if (System.DBNull.Value.Equals(dr[11]))
                        {
                            selectItem.QuestionAnswerId = 0;
                        }
                        else
                        {
                            selectItem.QuestionAnswerId = Convert.ToInt32(dr[11]);
                        }

                        if (System.DBNull.Value.Equals(dr[12]))
                        {
                            selectItem.ChoiceDescription = null;
                        }
                        else
                        {
                            selectItem.ChoiceDescription = Convert.ToString(dr[12]);
                        }

                        if (System.DBNull.Value.Equals(dr[13]))
                        {
                            selectItem.ChoiceSequenceNumber = null;
                        }
                        else
                        {
                            selectItem.ChoiceSequenceNumber = Convert.ToString(dr[13]);
                        }

                        quizList.Last().MulSelQuestion.Last().SelectionItems.Add(selectItem);
                        quizList.Last().MulSelQuestion.Last().ItemsNumber++;
                    }   
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return quizList;
        }

        /// <summary>
        /// Get a quiz list for a module that customers want to select
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<QuizTransfer> GetQuizForModule(int moduleId)
        {
            List<QuizTransfer> quizList = new List<QuizTransfer>();

            string currentQuestionType;
            int currentQuizId = 0;
            int currentQuestionId = 0;

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DISTINCT * FROM Quiz AS Q LEFT JOIN Question_Answer AS QA ON Q.quizId = QA.quizId"
                                + " LEFT JOIN Qustion_Answer_Multiple_Selection AS QAMS ON QA.questionAnswerId = QAMS.questionAnswerId"
                                + " WHERE Q.moduleId = @moduleId;";
            cmd.Parameters.Add("@moduleId", SqlDbType.Int).Value = moduleId;
            
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //add data to quiz table
                    //quizId, quizName, description, passingGrade
                    QuizTransfer quizTransfer = new QuizTransfer();

                    if (currentQuizId != Convert.ToInt32(dr[0]))
                    {
                        QuizAccess quiz = new QuizAccess();

                        if (System.DBNull.Value.Equals(dr[0]))
                        {
                            quiz.QuizId = 0;
                        }
                        else
                        {
                            quiz.QuizId = Convert.ToInt32(dr[0]);
                        }

                        currentQuizId = quiz.QuizId;

                        if (System.DBNull.Value.Equals(dr[1]))
                        {
                            quiz.QuizName = null;
                        }
                        else
                        {
                            quiz.QuizName = Convert.ToString(dr[1]);
                        }

                        if (System.DBNull.Value.Equals(dr[2]))
                        {
                            quiz.Description = null;
                        }
                        else
                        {
                            quiz.Description = Convert.ToString(dr[2]);
                        }

                        if (System.DBNull.Value.Equals(dr[3]))
                        {
                            quiz.PassingGrade = 0;
                        }
                        else
                        {
                            quiz.PassingGrade = Convert.ToInt32(dr[3]);
                        }

                        if (System.DBNull.Value.Equals(dr[4]))
                        {
                            quiz.ModuleId = 0;
                        }
                        else
                        {
                            quiz.ModuleId = Convert.ToInt32(dr[4]);
                        }

                        quizTransfer.QuizDescription = quiz;
                        //add quiz table to quizTransfer
                        if (currentQuizId != 0)
                        {
                            quizList.Add(quizTransfer);
                        }
                    }

                    //add data to Question_Answer table
                    //questionAnswerId, quizId, questionDescription, type, Answer
                    if (!System.DBNull.Value.Equals(dr[5]))
                    {
                        if (currentQuestionId != Convert.ToInt32(dr[5]))
                        {
                            QuestionAnswerAccess question = new QuestionAnswerAccess();
                            QuestionTransfer mulQuestion = new QuestionTransfer();

                            if (System.DBNull.Value.Equals(dr[5]))
                            {
                                question.QuestionAnswerId = 0;
                            }
                            else
                            {
                                question.QuestionAnswerId = Convert.ToInt32(dr[5]);
                            }

                            currentQuestionId = question.QuestionAnswerId;

                            if (System.DBNull.Value.Equals(dr[6]))
                            {
                                question.QuizId = 0;
                            }
                            else
                            {
                                question.QuizId = Convert.ToInt32(dr[6]);
                            }

                            if (System.DBNull.Value.Equals(dr[7]))
                            {
                                question.QuestionDescription = null;
                            }
                            else
                            {
                                question.QuestionDescription = Convert.ToString(dr[7]);
                            }

                            if (System.DBNull.Value.Equals(dr[8]))
                            {
                                question.Type = null;
                            }
                            else
                            {
                                question.Type = Convert.ToString(dr[8]);
                            }

                            currentQuestionType = question.Type;

                            if (System.DBNull.Value.Equals(dr[9]))
                            {
                                question.Answer = null;
                            }
                            else
                            {
                                question.Answer = Convert.ToString(dr[9]);
                            }

                            if (currentQuestionType == "bool")
                            {
                                quizList.Last().BoolQuestion.Add(question);
                                quizList.Last().BoolQuestionNumber++;
                            }
                            else
                            {
                                mulQuestion.QuestionAnswer = question;
                                quizList.Last().MulSelQuestion.Add(mulQuestion);
                                quizList.Last().MulSelQuestionNumber++;
                            }
                        }
                    }


                    //add data to Qustion_Answer_Multiple_Selection table
                    //mulSelectionId, questionAnswerId, choiceDescription, choiceSequenceNumber

                    if (!System.DBNull.Value.Equals(dr[10]))
                    {
                        QuestionSelectItem selectItem = new QuestionSelectItem();
                        selectItem.MulSelectionId = Convert.ToInt32(dr[10]);

                        if (System.DBNull.Value.Equals(dr[11]))
                        {
                            selectItem.QuestionAnswerId = 0;
                        }
                        else
                        {
                            selectItem.QuestionAnswerId = Convert.ToInt32(dr[11]);
                        }

                        if (System.DBNull.Value.Equals(dr[12]))
                        {
                            selectItem.ChoiceDescription = null;
                        }
                        else
                        {
                            selectItem.ChoiceDescription = Convert.ToString(dr[12]);
                        }

                        if (System.DBNull.Value.Equals(dr[13]))
                        {
                            selectItem.ChoiceSequenceNumber = null;
                        }
                        else
                        {
                            selectItem.ChoiceSequenceNumber = Convert.ToString(dr[13]);
                        }

                        quizList.Last().MulSelQuestion.Last().SelectionItems.Add(selectItem);
                        quizList.Last().MulSelQuestion.Last().ItemsNumber++;
                    }
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return quizList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        public bool UpdateQuiz(QuizTransfer quiz)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            int boolQuestionNumber = quiz.BoolQuestionNumber;
            int mulQuestionNumber = quiz.MulSelQuestionNumber;
            int itemNumber = 0;

            //update data in Quiz table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE Quiz SET quizName=@quizName, description=@description, passingGrade=@passingGrade, moduleId=@moduleId WHERE quizId=@quizId;";
            cmd.Parameters.AddWithValue("@quizName", quiz.QuizDescription.QuizName);
            cmd.Parameters.AddWithValue("@description", quiz.QuizDescription.Description);
            cmd.Parameters.AddWithValue("@passingGrade", quiz.QuizDescription.PassingGrade);
            cmd.Parameters.AddWithValue("@moduleId", quiz.QuizDescription.ModuleId);
            cmd.Parameters.AddWithValue("@quizId", quiz.QuizDescription.QuizId);
            db.changeData(cmd);

            //update data in bool question table
            for (int i = 0; i < boolQuestionNumber; i++)
            {
                cmd.CommandText = "UPDATE Question_Answer SET quizId="
                            + "@quizIdbool" + i
                            + ", questionDescription=@questionDescriptionbool" + i
                            + ", type=@typebool" + i
                            + ", Answer=@Answerbool" + i
                            + "  WHERE questionAnswerId=@questionAnswerIdbool" + i
                            + ";";
                cmd.Parameters.AddWithValue("@quizIdbool" + i, quiz.BoolQuestion.ElementAt(i).QuizId);
                cmd.Parameters.AddWithValue("@questionDescriptionbool" + i, quiz.BoolQuestion.ElementAt(i).QuestionDescription);
                cmd.Parameters.AddWithValue("@typebool" + i, quiz.BoolQuestion.ElementAt(i).Type);
                cmd.Parameters.AddWithValue("@Answerbool" + i, quiz.BoolQuestion.ElementAt(i).Answer);
                cmd.Parameters.AddWithValue("@questionAnswerIdbool" + i, quiz.BoolQuestion.ElementAt(i).QuestionAnswerId);
                db.changeData(cmd);
            }

            //update data in multiple question table
            for (int j = 0; j < mulQuestionNumber; j++)
            {
                cmd.CommandText = "UPDATE Question_Answer SET quizId=@quizIdmul" + j
                                + ", questionDescription=@questionDescriptionmul" + j
                                + ", type=@typemul" + j
                                + ", Answer=@Answermul" + j
                                + " WHERE questionAnswerId=@questionAnswerIdmul" + j
                                + ";";
                cmd.Parameters.AddWithValue("@quizIdmul" + j, quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.QuizId);
                cmd.Parameters.AddWithValue("@questionDescriptionmul" + j, quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.QuestionDescription);
                cmd.Parameters.AddWithValue("@typemul" + j, quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.Type);
                cmd.Parameters.AddWithValue("@Answermul" + j, quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.Answer);
                cmd.Parameters.AddWithValue("@questionAnswerIdmul" + j, quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.QuestionAnswerId);
                db.changeData(cmd);

                itemNumber = quiz.MulSelQuestion.ElementAt(j).ItemsNumber;
                //update data in Qustion_Answer_Multiple_Selection table
                for (int k = 0; k < itemNumber; k++)
                {
                    cmd.CommandText = "UPDATE Qustion_Answer_Multiple_Selection SET questionAnswerId=@questionAnswerId" + j + k
                                    + ", choiceDescription=@choiceDescription" + j + k
                                    + ", choiceSequenceNumber=@choiceSequenceNumber" + j + k
                                    + " WHERE mulSelectionId=@mulSelectionId" + j + k
                                    + ";";
                    cmd.Parameters.AddWithValue("@questionAnswerId" + j + k, quiz.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).QuestionAnswerId);
                    cmd.Parameters.AddWithValue("@choiceDescription" + j + k, quiz.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).ChoiceDescription);
                    cmd.Parameters.AddWithValue("@choiceSequenceNumber" + j + k, quiz.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).ChoiceSequenceNumber);
                    cmd.Parameters.AddWithValue("@mulSelectionId" + j + k, quiz.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).MulSelectionId);
                    db.changeData(cmd);
                }
            } 
            db.Close();
            return true;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizIdList"></param>
        /// <returns></returns>
        public bool Delete(List<Int32> quizIdList)
        {
            Int32 number;
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;


            number = quizIdList.Count;
            for (int i = 0; i < number; i++)
            {
                cmd.CommandText = "DELETE FROM Quiz WHERE quizId = @quizId" + i
                                + ";";
                cmd.Parameters.AddWithValue("@quizId" + i, quizIdList[i]);
                db.changeData(cmd);
            }
            db.Close();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool AddToModule(int quizId, int moduleId)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE Quiz SET moduleId=@moduleId WHERE quizId=@quizId;";
            cmd.Parameters.AddWithValue("@moduleId", moduleId);
            cmd.Parameters.AddWithValue("@quizId", quizId);
            db.changeData(cmd);

            db.Close();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizAccountAnswers"></param>
        /// <returns></returns>
        public bool SaveAccountAnswer(List<QuizAccountAnswerAccess> quizAccountAnswers)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;

            int number = quizAccountAnswers.Count;
            DateTime today = DateTime.Now; 

            //update data in Quiz table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            for (int i = 0; i < number; i++)
            {
                cmd.CommandText = "INSERT INTO Quiz_Account_Answer(questionAnswerId,accountId,answer,submitDate)VALUES(@questionAnswerId" + i
                                + ", @accountId" + i
                                + ", @answer" + i
                                + ", @submitDate" + i
                                + ");";
                cmd.Parameters.AddWithValue("@questionAnswerId" + i, quizAccountAnswers.ElementAt(i).QuestionAnswerId);
                cmd.Parameters.AddWithValue("@accountId" + i, quizAccountAnswers.ElementAt(i).AccountId);
                cmd.Parameters.AddWithValue("@answer" + i, quizAccountAnswers.ElementAt(i).Answer);
                cmd.Parameters.AddWithValue("@submitDate" + i, today);
                db.changeData(cmd);
            } 
            db.Close();
            return true;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizRecord"></param>
        /// <returns></returns>
        public bool SaveQuizRecord(QuizRecordAccess quizRecord)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            DateTime today;
            SqlDataReader dr;
            int grade = 0;
            bool isInsert = false;

            //update data in Quiz table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            //find whether there is a record in Quiz_record table
            cmd.CommandText = "SELECT highestGrade FROM Quiz_record WHERE quizId = @quizId AND accountId = @accountId;";
            cmd.Parameters.AddWithValue("@quizId", quizRecord.QuizId);
            cmd.Parameters.AddWithValue("@accountId", quizRecord.AccountId);
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TrainingAccess training = new TrainingAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        grade = 0;
                        isInsert = true;
                    }
                    else
                    {
                        grade = Convert.ToInt32(dr[0]);
                    }
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }

            if (grade <= quizRecord.HighestGrade)
            {
                grade = quizRecord.HighestGrade;
                today = quizRecord.HighGradeTime;
            }
            else
            {
                today = DateTime.Now;
            }

            if (isInsert == true)
            {
                //insert a new record in Quiz_record table
                cmd.CommandText = "INSERT INTO Quiz_record(accountId,quizId,attendTimes,highestGrade,highGradeTime)VALUES(@accountId, @quizId, @attendTimes, @highestGrade, @highGradeTime);";
                cmd.Parameters.AddWithValue("@accountId", quizRecord.AccountId);
                cmd.Parameters.AddWithValue("@quizId", quizRecord.QuizId);
                cmd.Parameters.AddWithValue("@attendTimes", today);
                cmd.Parameters.AddWithValue("@highestGrade", grade);
                cmd.Parameters.AddWithValue("@highGradeTime", today);
            }
            else if (grade != quizRecord.HighestGrade)
            {
                //update data in Quiz_record table
                cmd.CommandText = "UPDATE Quiz_record SET highestGrade = @highestGrade,highGradeTime = @highGradeTime WHERE quizId = @quizId AND accountId = @accountId ;";
                cmd.Parameters.AddWithValue("@highestGrade", grade);
                cmd.Parameters.AddWithValue("@highGradeTime", today);
                cmd.Parameters.AddWithValue("@quizId", quizRecord.QuizId);
                cmd.Parameters.AddWithValue("@accountId", quizRecord.AccountId);
            }
            
            db.changeData(cmd);
            db.Close();
            return true;
        }
    }
}
