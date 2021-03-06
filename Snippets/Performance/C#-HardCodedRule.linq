<Query Kind="Program">
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
//    // input data sample
//    IEnumerable<Model.ProcessingModel> models = new[]
//    {
//        new Model.ProcessingModel { InputA = 10M, InputB = 5M, Factor = 0.050M }
//    };

    // generate input data
    IEnumerable<Model.ProcessingModel> models = 
                        Enumerable.Range(0, 1000000)
                            .Select(n => new Model.ProcessingModel { InputA = n, InputB = n * 0.5M, Factor = 0.050M });
    
    var sw = Stopwatch.StartNew();
   
    // process input data
    IEnumerable<Model.ReportModel> results =
                    models
                        .Select(model =>
                                    {
                                        model.Result = model.InputA + model.InputB * model.Factor;
                                        model.Delta = Math.Abs((model.Result ?? 0M) - model.InputA);
                                        model.Description = @"Some description";
                                        return new Model.ReportModel { Σ = model.Result, Δ = model.Delta, λ = model.Description };
                                    })
                        .ToList();
    
    sw.Stop();

    string.Format("Time taken: {0}ms", sw.Elapsed.TotalMilliseconds).Dump();
    
    // merge results
    results
        .Zip(models, (result, model) => new { result, model })
        .Select(@group => 
                    new
                    {
                        @return = @group.result,
                        ResultModel = @group.model
                    })
        .Take(10)
        .Dump();
}
}

namespace Model
{
    public class ProcessingModel
    {
        public decimal InputA { get; set; }
        public decimal InputB { get; set; }
        public decimal Factor { get; set; }
        
        public decimal? Result { get; set; }
        public decimal? Delta { get; set; }
        public string Description { get; set; }
        public decimal? Addition { get; set; }
    }
    
    public class ReportModel
    {
        public decimal? Σ { get; set; }
        public decimal? Δ { get; set; }
        public string λ { get; set; }
    }