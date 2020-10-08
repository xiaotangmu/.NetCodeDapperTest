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
        /// ���񼯺�
        /// </summary>
        private IServiceCollection _services;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���÷���������ʱ������
        /// <para>
        /// �ڴ˷�������ӻ����÷���
        /// </para>
        /// </summary>
        /// <param name="services">���񼯺�</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ////������ÿ���ע��controller
            //services.Configure<TokenProviderOptions>(Configuration.GetSection("Jwt"));
            //TokenProviderOptions tokenOptions = new TokenProviderOptions();
            //Configuration.Bind("Jwt", tokenOptions);

            //JWTTokenValid(services);

            #region �������
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

            #region Swagger Api�ĵ�
            //ע��ʵ��ISwaggerProviderʹ��Ĭ������
            //services.AddSwaggerGen(options =>
            //{
            //    //"v1"ΪSwaggerOptions��RouteTemplate������{documentName}
            //    options.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "API�ĵ�",
            //        Description = "API�ӿ������ĵ�",
            //        //TermsOfService = "None",
            //        //Contact = new Contact { Name = "Arvin Lv", Email = "lxl@jadedragontech.com", Url = "http://www.jadedragontech.com" },
            //        //License = new License { Name = "Apache2.0", Url = "https://www.apache.org/licenses/LICENSE-2.0.html" }
            //    });

            //    //��ȡӦ�ó����·�� ����ƽ̨�� 
            //    var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
            //    //����XMLע���ļ�������·��
            //    var xmlPath = Path.Combine(basePath, "WebApi.xml");
            //    options.IncludeXmlComments(xmlPath);

            //    //�޸�doc�������Զ���ڵ㣨�������ݣ���ʹ�� SwaggerControllerGroup���ԣ�
            //    options.DocumentFilter<GroupDocFilter>(xmlPath);

            //});
            #endregion

            #region MVC����
            //services.AddMvc(options =>
            //{
            //    options.UseCentraRoutePrefix(new RouteAttribute(ApiPrefix));
            //})
            //    .AddJsonOptions(options =>
            //    {
            //        //�������jsonСд����
            //        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //    })
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            //    .AddDataAnnotationsLocalization(options =>
            //    {
            //        options.DataAnnotationLocalizerProvider = (type, factory) =>
            //            factory.Create("Language", Assembly.GetExecutingAssembly().FullName);
            //    });
            #endregion

            #region ����ȫ�򻯱��ػ����ã�Ĭ����������Ϊzh-CN

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
            //    //�Զ���������Ļ���Ӧ���࣬����Ϊ��λƥ��
            //    options.RequestCultureProviders.Insert(0, new CustomCultureProvider());
            //});
            //services.AddMemoryCache();

            #endregion

            #region ����https����
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
