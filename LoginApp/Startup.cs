using BLL;
using BLL.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoginApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 服务集合
        /// </summary>
        private IServiceCollection _services;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务，在运行时被调用
        /// <para>
        /// 在此方法内添加或配置服务
        /// </para>
        /// </summary>
        /// <param name="services">服务集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ////添加配置可以注入controller
            //services.Configure<TokenProviderOptions>(Configuration.GetSection("Jwt"));
            //TokenProviderOptions tokenOptions = new TokenProviderOptions();
            //Configuration.Bind("Jwt", tokenOptions);

            //JWTTokenValid(services);

            #region 跨域策略
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                      .AllowAnyOrigin() //.WithOrigins(new string[] {"",""})
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                .Build());
            });
            #endregion

            #region Swagger Api文档
            //注入实现ISwaggerProvider使用默认设置
            //services.AddSwaggerGen(options =>
            //{
            //    //"v1"为SwaggerOptions中RouteTemplate方法的{documentName}
            //    options.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "API文档",
            //        Description = "API接口描述文档",
            //        //TermsOfService = "None",
            //        //Contact = new Contact { Name = "Arvin Lv", Email = "lxl@jadedragontech.com", Url = "http://www.jadedragontech.com" },
            //        //License = new License { Name = "Apache2.0", Url = "https://www.apache.org/licenses/LICENSE-2.0.html" }
            //    });

            //    //获取应用程序根路径 （跨平台） 
            //    var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
            //    //设置XML注释文件的完整路径
            //    var xmlPath = Path.Combine(basePath, "WebApi.xml");
            //    options.IncludeXmlComments(xmlPath);

            //    //修改doc，增加自定义节点（分组内容，需使用 SwaggerControllerGroup特性）
            //    options.DocumentFilter<GroupDocFilter>(xmlPath);

            //});
            #endregion

            #region MVC服务
            //services.AddMvc(options =>
            //{
            //    options.UseCentraRoutePrefix(new RouteAttribute(ApiPrefix));
            //})
            //    .AddJsonOptions(options =>
            //    {
            //        //解决返回json小写问题
            //        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //    })
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            //    .AddDataAnnotationsLocalization(options =>
            //    {
            //        options.DataAnnotationLocalizerProvider = (type, factory) =>
            //            factory.Create("Language", Assembly.GetExecutingAssembly().FullName);
            //    });
            #endregion

            #region 语言全球化本地化配置，默认语言设置为zh-CN

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture(CultureHelper.GetDefaultCulture());
            //    IList<CultureInfo> supportCulture = new List<CultureInfo>();
            //    foreach (Culture culture in CultureHelper.Cultures)
            //    {
            //        supportCulture.Add(new CultureInfo(culture.Code));
            //    }
            //    options.SupportedCultures = CultureHelper.GetSupportCultureInfo();
            //    options.SupportedUICultures = options.SupportedCultures;
            //    //自定义的语言文化供应商类，并作为首位匹配
            //    options.RequestCultureProviders.Insert(0, new CustomCultureProvider());
            //});
            //services.AddMemoryCache();

            #endregion

            #region 用于https请求
            //services.AddAntiforgery(options=> {
            //    options.Cookie.Name = "_af";
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.HeaderName = "X-XSRF-TOKEN";
            //});
            #endregion

            services.AddSingleton(Configuration);
            services.AddScoped<UserService, UserServiceImpl>();
            services.AddScoped(typeof(DapperConn.Dao.UserDao));

            services.AddHttpClient();

            services.AddControllers();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
