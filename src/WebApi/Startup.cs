using Microsoft.OpenApi.Models;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v0.0.1",
                    Title = "OpenMir2 API",
                    Description = "接口文档说明",
                    Contact = new OpenApiContact()
                    {
                        Name = "",
                        Email = "",
                        Url = null
                    }
                });
                options.IncludeXmlComments("WebApi.xml", true);
                
                //定义JwtBearer认证方式一
                options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                {
                    Description = "这是方式一(直接在输入框中输入认证信息，不需要在开头添加Bearer)",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                //定义JwtBearer认证方式二
                //options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                //{
                //    Description = "这是方式二(JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）)",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});
                //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
                var scheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
                };
                //注册全局认证（所有的接口都可以使用认证）
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [scheme] = new string[0]
                });
            });
            
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}