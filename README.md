## Blazor Server ASP .Net Core 5 Survey

### Create custom surveys and results

- Database: [SQLite](https://www.sqlite.org/index.html) until [Polemo MySQL](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1088) is ready for .NET 5
- Charts: [ChartJS](https://www.chartjs.org/)
- User Id: Using md5 hashed [Cloudflare trace](https://www.cloudflare.com/cdn-cgi/trace) information to detect same user
- Reload: Catching connection lost using MemoryCache
- Authorization: For detailed results and internal surveys: TBD
- _Lizence: [GNU General Public License v3.0](../blob/master/LICENSE)_

[Test installation](https://www.pax77.org/survey/)
![Survey](/images/survey_github.png "Survey/Result/Create")

#### Get Started

- Copy [samplesurveyconfig.json](../master/pax.blazor.survey/samplesurveyconfig.json) to a folder outside the repository
- Modify the copy to your needs
- Change path in [SurveyData.cs](../master/pax.blazor.survey/Data/SurveyData.cs) line 11 to the copy
    ```
    public static string configfile = "samplesurveyconfig.json";
    ```
#### TODO
- Allow Users to change answers after Submit
- only show Result after submit
- Answer types (integer, float, date)
- Question Icons
- Test Project

#### How to Engage, Contribute, and Give Feedback

Feel free to open Issues and/or create pull-requests
